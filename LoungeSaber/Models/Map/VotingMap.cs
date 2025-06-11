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