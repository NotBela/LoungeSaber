using System;
using LoungeSaber.Models.Packets.UserPackets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoungeSaber.Models.Packets
{
    public abstract class UserPacket : Packet
    {
        [JsonProperty("type")]
        public abstract UserPacketTypes PacketType { get; }

        public enum UserPacketTypes
        {
            JoinRequest,
            Vote
        }
    }
}