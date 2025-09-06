﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LoungeSaber.Models.Map;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.PauseMenu;
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
        
        [Inject] private readonly ServerListener _serverListener = null!;
        
        public bool InMatch { get; private set; } = false;

        [CanBeNull] public Models.UserInfo.UserInfo Opponent { get; private set; }

        public event Action<LevelCompletionResults, StandardLevelScenesTransitionSetupDataSO> OnLevelCompleted;

        public void SetOpponent(Models.UserInfo.UserInfo opponent) => Opponent = opponent;
        
        public void StartMatch(VotingMap level, DateTime unpauseTime, bool proMode)
        {
            if (InMatch) 
                return;
            
            InMatch = true;
            
            var beatmapLevel = level.GetBeatmapLevel() ?? throw new Exception("Could not get beatmap level!");
            var difficulty = level.GetBaseGameDifficultyType();

            _menuTransitionsHelper.StartStandardLevel(
                "Solo", 
                beatmapLevel.GetBeatmapKeys().First(i => i.beatmapCharacteristic.serializedName == "Standard" && i.difficulty == difficulty),
                beatmapLevel,
                _playerDataModel.playerData.overrideEnvironmentSettings,
                _playerDataModel.playerData.colorSchemesSettings.overrideDefaultColors ? _playerDataModel.playerData.colorSchemesSettings.GetSelectedColorScheme() : null,
                true,
                beatmapLevel.GetColorScheme(beatmapLevel.GetCharacteristics().First(i => i.serializedName == "Standard"), level.GetBaseGameDifficultyType()),
                new GameplayModifiers(GameplayModifiers.EnergyType.Bar, true, false, false, GameplayModifiers.EnabledObstacleType.All, false, false, false, false, GameplayModifiers.SongSpeed.Normal, false, false, proMode, false, false),
                _playerDataModel.playerData.playerSpecificSettings,
                null,
                EnvironmentsListModel.CreateFromAddressables(),
                "Menu",
                false,
                true,
                null,
                // TODO: fix restart button being visible
                diContainer => AfterSceneSwitchToGameplayCallback(diContainer, unpauseTime),
                AfterSceneSwitchCallback,
                null
            );
        }

        private void AfterSceneSwitchCallback(StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSo, LevelCompletionResults levelCompletionResults)
        {
            InMatch = false;
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

        public void Initialize()
        {
            _serverListener.OnDisconnected += OnDisconnect;
        }

        private void OnDisconnect()
        {
            if (InMatch && SceneManager.GetActiveScene().name == "GameCore")
            {
                _menuTransitionsHelper.StopStandardLevel();
            }
        }

        public void Dispose()
        {
            _serverListener.OnDisconnected -= OnDisconnect;
        }
    }
}