using System;
using LoungeSaber.Models.Packets.ServerPackets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoungeSaber.Models.Packets
{
    public abstract class ServerPacket : Packet
    {
        [JsonProperty("type")]
        public abstract ServerPacket.ServerPacketTypes PacketType { get; }
    
        public static ServerPacket Deserialize(string json)
        {
            var jobj = JObject.Parse(json);
        
            if (!jobj.TryGetValue("type", out var packetTypeJToken))
                throw new Exception("Could not deserialize packet!");
        
            if (!Enum.TryParse<ServerPacket.ServerPacketTypes>(packetTypeJToken.ToObject<string>(), out var userPacketType))
                throw new Exception("Could not deserialize packet type!");

            switch (userPacketType)
            {
                case ServerPacketTypes.JoinedQueue:
                    return JsonConvert.DeserializeObject<JoinedQueue>(json);
                default:
                    throw new Exception("Could not get packet type!");
            }
        }
    
        public enum ServerPacketTypes
        {
            JoinedQueue
        }
    }
}