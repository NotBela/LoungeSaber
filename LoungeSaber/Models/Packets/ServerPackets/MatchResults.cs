﻿using LoungeSaber.Models.Packets.UserPackets;
using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets
{
    public class MatchResults : ServerPacket
    {
        public override ServerPacketTypes PacketType => ServerPacketTypes.MatchResults;
    
        [JsonProperty("opponentScore")]
        public readonly ScoreSubmissionPacket OpponentScore;
        
        [JsonProperty("yourScore")]
        public readonly ScoreSubmissionPacket YourScore;

        [JsonProperty("winner")]
        public readonly MatchWinner Winner;
    
        [JsonProperty("mmrChange")]
        public readonly int MMRChange;

        [JsonProperty("newOpponentUserInfo")] 
        public readonly UserInfo.UserInfo NewOpponentUserInfo;
        
        [JsonProperty("newClientUserInfo")]
        public readonly UserInfo.UserInfo NewClientUserInfo;

        [JsonConstructor]
        public MatchResults(ScoreSubmissionPacket opponentScore, ScoreSubmissionPacket yourScore, MatchWinner winner, int mmrChange, UserInfo.UserInfo newOpponentUserInfo, UserInfo.UserInfo newClientUserInfo)
        {
            OpponentScore = opponentScore;
            YourScore = yourScore;
            Winner = winner;
            MMRChange = mmrChange;
            NewOpponentUserInfo = newOpponentUserInfo;
            NewClientUserInfo = newClientUserInfo;
        }

        public enum MatchWinner
        {
            You,
            Opponent
        }
    }
}