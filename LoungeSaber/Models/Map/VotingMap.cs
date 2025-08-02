using BeatSaberMarkupLanguage.Components;
using JetBrains.Annotations;
using Newtonsoft.Json;
using SongCore;

namespace LoungeSaber.Models.Map
{
    public class VotingMap
    {
        [JsonProperty("hash")]
        public readonly string Hash;

        [JsonProperty("difficulty")]
        public readonly DifficultyType Difficulty;

        [JsonProperty("category")]
        public readonly CategoryType Category;

        [JsonConstructor]
        public VotingMap(string hash, DifficultyType difficulty, CategoryType category)
        {
            Hash = hash;
            Difficulty = difficulty;
            Category = category;
        }

        [CanBeNull] public BeatmapLevel GetBeatmapLevel() => Loader.GetLevelByHash(Hash);

        public BeatmapDifficulty GetBaseGameDifficultyType() => Difficulty switch
        {
            DifficultyType.Easy => BeatmapDifficulty.Easy,
            DifficultyType.Normal => BeatmapDifficulty.Normal,
            DifficultyType.Hard => BeatmapDifficulty.Hard,
            DifficultyType.Expert => BeatmapDifficulty.Expert,
            DifficultyType.ExpertPlus => BeatmapDifficulty.ExpertPlus,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        public BeatmapDifficultyMask GetBaseGameDifficultyTypeMask() => GetBaseGameDifficultyType() switch
        {
            BeatmapDifficulty.Easy => BeatmapDifficultyMask.Easy,
            BeatmapDifficulty.Normal => BeatmapDifficultyMask.Normal,
            BeatmapDifficulty.Hard => BeatmapDifficultyMask.Hard,
            BeatmapDifficulty.Expert => BeatmapDifficultyMask.Expert,
            BeatmapDifficulty.ExpertPlus => BeatmapDifficultyMask.ExpertPlus,
            _ => BeatmapDifficultyMask.All
        };

        public BeatmapKey GetBeatmapKey() => GetBeatmapLevel()?.GetBeatmapKeys().First(i =>
            i.beatmapCharacteristic.serializedName == "Standard" && i.difficulty == GetBaseGameDifficultyType()) ?? throw new Exception("Could not find beatmap key!");

        public enum CategoryType
        {
            Acc,
            MidSpeed,
            Tech,
            Balanced,
            Speed,
            Extreme
        }
    
        public enum DifficultyType
        {
            Easy,
            Normal,
            Hard,
            Expert,
            ExpertPlus
        }
    }
}