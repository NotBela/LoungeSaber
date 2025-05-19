using LoungeSaber.Models.Maps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoungeSaber_Server.Models.Maps
{
    public class PlaylistMap
    {
        public PlaylistMap(string hash, MapDifficulty[] difficulties)
        {
            Hash = hash;
            Difficulties = difficulties;
        }

        [JsonProperty("hash")] 
        public string Hash { get; set; }

        [JsonProperty("difficulties")] 
        public MapDifficulty[] Difficulties { get; set; }

        public JObject Serialize() => JObject.FromObject(this);
    }
}