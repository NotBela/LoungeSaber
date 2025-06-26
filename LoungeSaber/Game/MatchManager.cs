using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoungeSaber.Models.Map;
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

        public void StartMatch(VotingMap level, DateTime unpauseTime, bool proMode)
        {
            if (InMatch) 
                return;
            
            InMatch = true;
            
            var beatmapLevel = level.GetBeatmapLevel() ?? throw new Exception("Could not get beatmap level!");
            BeatmapDifficulty difficulty;
            
            switch (level.Difficulty)
            {
                case VotingMap.DifficultyType.Easy:
                    difficulty = BeatmapDifficulty.Easy;
                    break;
                case VotingMap.DifficultyType.Normal:
                    difficulty = BeatmapDifficulty.Normal;
                    break;
                case VotingMap.DifficultyType.Hard:
                    difficulty = BeatmapDifficulty.Hard;
                    break;
                case VotingMap.DifficultyType.Expert:
                    difficulty = BeatmapDifficulty.Expert;
                    break;
                default:
                    difficulty = BeatmapDifficulty.ExpertPlus;
                    break;
            }

            _menuTransitionsHelper.StartStandardLevel(
                "Solo",
                beatmapLevel.GetBeatmapKeys().First(i => i.beatmapCharacteristic.serializedName == "Standard" && i.difficulty == difficulty),
                beatmapLevel,
                _playerDataModel.playerData.overrideEnvironmentSettings,
                _playerDataModel.playerData.colorSchemesSettings.overrideDefaultColors ? _playerDataModel.playerData.colorSchemesSettings.GetSelectedColorScheme() : null,
                null,
                new GameplayModifiers(GameplayModifiers.EnergyType.Bar, true, false, false, GameplayModifiers.EnabledObstacleType.All, false, false, false, false, GameplayModifiers.SongSpeed.Normal, false, false, proMode, false, false),
                _playerDataModel.playerData.playerSpecificSettings,
                null,
                EnvironmentsListModel.CreateFromAddressables(),
                "Menu",
                false,
                true,
                null,
                // TODO: fix restart button being visible
                (diContainer) => AfterSceneSwitchToGameplayCallback(diContainer, unpauseTime),
                null,
                null
                );
        }
        
        async void AfterSceneSwitchToGameplayCallback(DiContainer diContainer, DateTime unpauseTime)
        {
            try
            {
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