using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets
{
    public class JoinResponse : ServerPacket
    {
        public override ServerPacketTypes PacketType => ServerPacketTypes.JoinResponse;
        
        [JsonProperty("successful")]
        public bool Successful { get; private set; }
        
        [JsonProperty("message")]
        public string Message { get; private set; }
        
        public JoinResponse(bool successful, string message)
        {
            Successful = successful;
            Message = message;
        }
    }
}