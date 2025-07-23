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

        public BeatmapDifficultyMask GetBaseGameDifficultyType() => Difficulty switch
        {
            DifficultyType.Easy => BeatmapDifficultyMask.Easy,
            DifficultyType.Normal => BeatmapDifficultyMask.Normal,
            DifficultyType.Hard => BeatmapDifficultyMask.Hard,
            DifficultyType.Expert => BeatmapDifficultyMask.Expert,
            DifficultyType.ExpertPlus => BeatmapDifficultyMask.ExpertPlus,
            _ => throw new ArgumentOutOfRangeException()
        };

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