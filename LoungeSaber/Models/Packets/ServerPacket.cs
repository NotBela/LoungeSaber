using System;
using LoungeSaber_Server.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.ServerPackets.Match;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoungeSaber.Models.Packets
{
    public abstract class ServerPacket : Packet
    {
        [JsonProperty("type")]
        public abstract ServerPacketTypes PacketType { get; }
    
        public static ServerPacket Deserialize(string json)
        {
            var jobj = JObject.Parse(json);
        
            if (!jobj.TryGetValue("type", out var packetTypeJToken))
                throw new Exception("Could not deserialize packet!");
        
            if (!Enum.TryParse<ServerPacket.ServerPacketTypes>(packetTypeJToken.ToObject<string>(), out var userPacketType))
                throw new Exception("Could not deserialize packet type!");

            switch (userPacketType)
            {
                case ServerPacketTypes.JoinResponse:
                    return JsonConvert.DeserializeObject<JoinResponse>(json);
                case ServerPacketTypes.MatchCreated:
                    return JsonConvert.DeserializeObject<MatchCreatedPacket>(json);
                case ServerPacketTypes.OpponentVoted:
                    return JsonConvert.DeserializeObject<OpponentVoted>(json);
                case ServerPacketTypes.MatchStarted:
                    return JsonConvert.DeserializeObject<MatchStarted>(json);
                case ServerPacketTypes.MatchResults:
                    return JsonConvert.DeserializeObject<MatchResultsPacket>(json);
                case ServerPacketTypes.PrematureMatchEnd:
                    return JsonConvert.DeserializeObject<PrematureMatchEnd>(json);
                default:
                    throw new Exception("Could not get packet type!");
            }
        }
    
        public enum ServerPacketTypes
        {
            JoinResponse,
            MatchCreated,
            OpponentVoted,
            MatchStarted,
            MatchResults,
            PrematureMatchEnd,
            EventStarted
        }
    }
}