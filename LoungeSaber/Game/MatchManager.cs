using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using JetBrains.Annotations;
using LoungeSaber_Server.Models.Packets.ServerPackets;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Map;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.Disconnect;
using LoungeSaber.UI.BSML.PauseMenu;
using LoungeSaber.UI.FlowCoordinators;
using SiraUtil.Logging;
using UnityEngine.SceneManagement;
using Zenject;

namespace LoungeSaber.Game
{
    public class MatchManager : IInitializable, IDisposable
    {
        [Inject] private readonly MenuTransitionsHelper _menuTransitionsHelper = null!;
        [Inject] private readonly PlayerDataModel _playerDataModel = null!;
        [Inject] private readonly SiraLog _siraLog = null!;
        
        [Inject] private readonly IServerListener _serverListener = null!;
        
        public bool InMatch { get; private set; } = false;

        [CanBeNull] public Models.UserInfo.UserInfo Opponent { get; private set; }

        public event Action<LevelCompletionResults, StandardLevelScenesTransitionSetupDataSO> OnLevelCompleted;

        public event Action<string> OnLevelIncomplete;

        private bool _shouldShowDisconnectScreen = false;

        public void SetOpponent(Models.UserInfo.UserInfo opponent) => Opponent = opponent;
        
        public void StartMatch(VotingMap level, DateTime unpauseTime, bool proMode)
        {
            if (InMatch) 
                return;
            
            InMatch = true;
            
            var beatmapLevel = level.GetBeatmapLevel() ?? throw new Exception("Could not get beatmap level!");

            _menuTransitionsHelper.StartStandardLevel(
                "Solo",
                level.GetBeatmapKey(),
                beatmapLevel,
                _playerDataModel.playerData.overrideEnvironmentSettings,
                _playerDataModel.playerData.colorSchemesSettings.overrideDefaultColors ? _playerDataModel.playerData.colorSchemesSettings.GetSelectedColorScheme() : null,
                null,
                new GameplayModifiers(GameplayModifiers.EnergyType.Bar, true, false, false, GameplayModifiers.EnabledObstacleType.All, false, false, false, false, GameplayModifiers.SongSpeed.Normal, false, false, proMode, false, false),
                _playerDataModel.playerData.playerSpecificSettings,
                null,
                //TODO: fix this sometimes causing an exception because of creating from addressables
                EnvironmentsListModel.CreateFromAddressables(),
                "Menu",
                false,
                true,
                null,
                diContainer => AfterSceneSwitchToGameplayCallback(diContainer, unpauseTime),
                AfterSceneSwitchToMenuCallback,
                null
                );
        }

        private void AfterSceneSwitchToMenuCallback(StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSo, LevelCompletionResults levelCompletionResults)
        {
            InMatch = false;
            
            if (_shouldShowDisconnectScreen)
            {
                _shouldShowDisconnectScreen = false;
                
                OnLevelIncomplete?.Invoke("Disconnected");
                return;
            }
            
            OnLevelCompleted?.Invoke(levelCompletionResults, standardLevelScenesTransitionSetupDataSo);
        }

        private async void AfterSceneSwitchToGameplayCallback(DiContainer diContainer, DateTime unpauseTime)
        {
            try
            {
                diContainer.Resolve<PauseMenuViewController>().SetMatchStartingTime(unpauseTime);
                
                var startingMenuController = diContainer.TryResolve<MatchStartUnpauseController>() ?? throw new Exception("Could not resolve StartingPauseMenuController");

                await startingMenuController.UnpauseLevelAtTime(unpauseTime);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        private void StopLevel()
        {
            _shouldShowDisconnectScreen = true;
            
            if (InMatch && SceneManager.GetActiveScene().name == "GameCore")
                _menuTransitionsHelper.StopStandardLevel();
        }

        public void Initialize()
        {
            _serverListener.OnDisconnected += OnDisconnect;
            _serverListener.OnPrematureMatchEnd += OnPrematureMatchEnd;
        }

        private void OnPrematureMatchEnd(PrematureMatchEnd prematureMatchEnd)
        {
            StopLevel();
            
            OnLevelIncomplete?.Invoke("Opponent Disconnected");
        }

        private void OnDisconnect()
        {
            StopLevel();
        }

        public void Dispose()
        {
            _serverListener.OnDisconnected -= OnDisconnect;
        }
    }
}