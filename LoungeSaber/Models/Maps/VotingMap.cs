using Newtonsoft.Json;

namespace LoungeSaber.Models.Maps
{
    public class VotingMap
    {
        public VotingMap(string hash, MapDifficulty difficulty)
        {
            Hash = hash;
            Characteristic = difficulty.Characteristic;
            Difficulty = difficulty.Difficulty;
            Category = difficulty.Category;
        }

        [JsonProperty("hash")] public string Hash { get; set; }

        [JsonProperty("characteristic")] public string Characteristic { get; set; }

        [JsonProperty("difficulty")] public string Difficulty { get; set; }

        [JsonProperty("category")] public MapDifficulty.MapCategory Category { get; set; }
    }
}