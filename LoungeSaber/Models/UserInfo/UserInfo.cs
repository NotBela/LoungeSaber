using JetBrains.Annotations;
using Newtonsoft.Json;

namespace LoungeSaber.Models.UserInfo
{
    [method: JsonConstructor]
    public class UserInfo(string username, string userId, int mmr, DivisionInfo division, [CanBeNull] Badge badge, long rank, [CanBeNull] string discordId, bool banned)
    {
        [JsonProperty("username")]
        public string Username { get; private set; } = username;

        [JsonProperty("userId")]
        public string UserId { get; private set; } = userId;

        [JsonProperty("mmr")]
        public int Mmr { get; private set; } = mmr;

        [JsonProperty("badge")] [CanBeNull] public Badge Badge { get; private set; } = badge;

        [JsonProperty("rank")]
        public long Rank { get; private set; } = rank;

        [JsonProperty("discordId")] [CanBeNull] public string DiscordId { get; private set; } = discordId;

        [JsonProperty("division")] public DivisionInfo Division { get; private set; } = division;

        [JsonProperty("banned")] public bool Banned { get; private set; } = banned;

        public string GetFormattedUserName()
        {
            if (Badge == null) return Username;
            
            var formatted = $"<color={Badge.ColorCode}>{Username}</color>";
            return !Badge.Bold ? formatted : $"<b>{formatted}</b>";
        }
    }
}