using System;
using LoungeSaber_Server.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoungeSaber.Models.Networking
{
    public class UserPacket
    {
        [JsonProperty("PacketType")]
        public readonly PacketType Type;
        public readonly JObject JsonData;
    
        public UserPacket(PacketType packetType, JObject data)
        {
            Type = packetType;

            JsonData = data;
        }

        public string Serialize() => JsonConvert.SerializeObject(this);
    
        public enum PacketType
        {
            VoteOnMap,
            PostScore,
            Join,
            Leave
        }
    }
}