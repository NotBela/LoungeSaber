using LoungeSaber.Models.Map;
using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets
{
    public class MatchCreatedPacket : ServerPacket
    {
        public override ServerPacketTypes PacketType => ServerPacketTypes.MatchCreated;
    
        [JsonProperty("votingOptions")]
        public readonly VotingMap[] Maps;
    
        [JsonProperty("opponent")]
        public readonly UserInfo.UserInfo Opponent;

        [JsonConstructor]
        public MatchCreatedPacket(VotingMap[] maps, UserInfo.UserInfo opponent)
        {
            Maps = maps;
            Opponent = opponent;
        }
    }
}