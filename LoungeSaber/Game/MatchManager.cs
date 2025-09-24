using IPA.Loader;
using JetBrains.Annotations;
using LoungeSaber.Models.Map;
using LoungeSaber.UI.BSML.PauseMenu;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Game
{
    public class MatchManager
    {
        [Inject] private readonly MenuTransitionsHelper _menuTransitionsHelper = null;
        [Inject] private readonly PlayerDataModel _playerDataModel = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        public bool InMatch { get; private set; } = false;

        [CanBeNull] public Models.UserInfo.UserInfo Opponent { get; set; }

        public event Action<LevelCompletionResults, StandardLevelScenesTransitionSetupDataSO> OnLevelCompleted;
        
        private Action _menuSwitchCallback = null;
        
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

        public void StopMatch(Action menuSwitchCallback = null)
        {
            _menuSwitchCallback = menuSwitchCallback;
            
            if (InMatch)
                _menuTransitionsHelper.StopStandardLevel();
        }

        private void AfterSceneSwitchToMenuCallback(StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSo, LevelCompletionResults levelCompletionResults)
        {
            InMatch = false;
            
            _menuSwitchCallback?.Invoke();

            if (_menuSwitchCallback == null)
            {
                OnLevelCompleted?.Invoke(levelCompletionResults, standardLevelScenesTransitionSetupDataSo);
                return;
            }
            
            _menuSwitchCallback = null;
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
    }
}