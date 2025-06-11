using JetBrains.Annotations;
using Newtonsoft.Json;

namespace LoungeSaber.Models.Client
{
    public class UserInfo
    {
        [JsonProperty("username")]
        public string Username { get; private set; }
    
        [JsonProperty("userId")]
        public string UserId { get; private set; }
    
        [JsonProperty("mmr")]
        public int Mmr { get; private set; }
    
        [JsonProperty("badge")] [CanBeNull] public Badge Badge { get; private set; }
    
        [JsonConstructor]
        public UserInfo(string username, string userId, int mmr, [CanBeNull] Badge badge)
        {
            Username = username;
            UserId = userId;
            Mmr = mmr;
            Badge = badge;
        }

        public string GetFormattedBadgeName()
        {
            if (Badge == null) return Username;
            
            var formatted = $"<color={Badge.ColorCode}>{Badge.Name}</color>";
            return !Badge.Bold ? formatted : $"<b>{formatted}</b>";
        }
    }
}