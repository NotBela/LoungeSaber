using Newtonsoft.Json;

namespace LoungeSaber.Models.Maps
{
    public class MapDifficulty
    {
        public MapDifficulty(string characteristic, string difficulty, MapDifficulty.MapCategory category)
        {
            Characteristic = characteristic;
            Difficulty = difficulty;
            Category = category;
        }

        [JsonProperty("characteristic")]
        public string Characteristic { get; private set; }

        [JsonProperty("name")]
        public string Difficulty { get; private set; }

        [JsonIgnore] public MapCategory Category { get; private set; }
    
        public enum MapCategory
        {
            Speed,
            Midspeed,
            Acc,
            Tech,
            Balanced,
            Extreme,
            Unknown
        }
    }
}