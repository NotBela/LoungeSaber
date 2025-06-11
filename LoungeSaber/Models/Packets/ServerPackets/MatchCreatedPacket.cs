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
        public readonly Client.UserInfo Opponent;

        [JsonConstructor]
        public MatchCreatedPacket(VotingMap[] maps, Client.UserInfo opponent)
        {
            Maps = maps;
            Opponent = opponent;
        }
    }
}