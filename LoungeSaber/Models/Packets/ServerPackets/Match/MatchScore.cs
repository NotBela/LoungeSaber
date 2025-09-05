using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets.Match;

public record MatchScore
{
    [JsonConstructor]
    public MatchScore(UserInfo.UserInfo User, Score Score)
    {
        this.User = User;
        this.Score = Score;
    }

    [JsonProperty("user")]
    public UserInfo.UserInfo User { get; }
    
    [JsonProperty("score")]
    public Score Score { get; }
}