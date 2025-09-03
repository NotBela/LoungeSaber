using System;
using LoungeSaber.Models.Map;
using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets
{
    [method: JsonConstructor]
    public class MatchStarted(
        VotingMap mapSelected,
        DateTime transitionToGameTime,
        DateTime startingTime,
        UserInfo.UserInfo opponent) : ServerPacket
    {
        public override ServerPacketTypes PacketType => ServerPacketTypes.MatchStarted;

        [JsonProperty("map")]
        public readonly VotingMap MapSelected = mapSelected;

        [JsonProperty("transitionToGameTime")]
        public readonly DateTime TransitionToGameTime = transitionToGameTime;
    
        [JsonProperty("startingTime")]
        public readonly DateTime StartingTime = startingTime;

        [JsonProperty("opponent")] public readonly UserInfo.UserInfo Opponent = opponent;
    }
}