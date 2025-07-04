using JetBrains.Annotations;
using Newtonsoft.Json;

namespace LoungeSaber.Models.UserInfo
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
    
        [JsonProperty("rank")]
        public long Rank { get; private set; }
    
        [JsonProperty("discordId")] [CanBeNull] public string DiscordId { get; private set; }
    
        [JsonConstructor]
        public UserInfo(string username, string userId, int mmr, [CanBeNull] Badge badge, long rank, [CanBeNull] string discordId)
        {
            Username = username;
            UserId = userId;
            Mmr = mmr;
            Badge = badge;
            Rank = rank;
            DiscordId = discordId;
        }
        
        public string GetFormattedUserName()
        {
            if (Badge == null) return Username;
            
            var formatted = $"<color={Badge.ColorCode}>{Username}</color>";
            return !Badge.Bold ? formatted : $"<b>{formatted}</b>";
        }
    }
}