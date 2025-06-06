using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.UserPackets
{
    public class JoinRequestPacket : UserPacket
    {
        public override UserPacketTypes PacketType => UserPacketTypes.JoinRequest;
        
        [JsonProperty("username")]
        public string UserName { get; private set; }
    
        [JsonProperty("userId")]
        public string UserId { get; private set; }

        [JsonConstructor]
        public JoinRequestPacket(string userName, string userId)
        {
            this.UserName = userName;
            this.UserId = userId;
        }
    }
}