using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets
{
    public class OpponentVoted : ServerPacket
    {
        public override ServerPacketTypes PacketType => ServerPacketTypes.OpponentVoted;
    
        [JsonProperty("vote")]
        public readonly int VoteIndex;

        [JsonConstructor]
        public OpponentVoted(int voteIndex)
        {
            VoteIndex = voteIndex;
        }
    }
}