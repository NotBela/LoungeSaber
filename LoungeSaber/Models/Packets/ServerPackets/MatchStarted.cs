using System;
using LoungeSaber.Models.Map;
using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets
{
    [method: JsonConstructor]
    public class MatchStarted(
        VotingMap mapSelected,
        int transitionToGameWait,
        int startingWait,
        UserInfo.UserInfo opponent) : ServerPacket
    {
        public override ServerPacketTypes PacketType => ServerPacketTypes.MatchStarted;

        [JsonProperty("map")]
        public readonly VotingMap MapSelected = mapSelected;

        [JsonProperty("transitionToGameTime")]
        public readonly int TransitionToGameWait = transitionToGameWait;
    
        [JsonProperty("startingTime")]
        public readonly int StartingWait = startingWait;

        [JsonProperty("opponent")] 
        public readonly UserInfo.UserInfo Opponent = opponent;
    }
}