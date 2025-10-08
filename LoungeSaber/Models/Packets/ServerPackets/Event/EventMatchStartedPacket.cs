using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets.Event;

[method: JsonConstructor]
public class EventMatchStartedPacket(MatchStarted matchData, UserInfo.UserInfo opponent) : ServerPacket
{
    public override ServerPacketTypes PacketType => ServerPacketTypes.EventMatchCreated;
    
    [JsonProperty("matchData")]
    public readonly MatchStarted MatchData = matchData;
    
    [JsonProperty("opponent")]
    public readonly UserInfo.UserInfo Opponent = opponent;
}