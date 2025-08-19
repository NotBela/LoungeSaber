﻿using LoungeSaber.Models.Packets.ServerPackets.Match;
using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets;


public class MatchResultsPacket : ServerPacket
{
    public override ServerPacketTypes PacketType => ServerPacketTypes.MatchResults;

    [JsonProperty("winningScore")] public readonly MatchScore WinnerScore;

    [JsonProperty("losingScore")] public readonly MatchScore LoserScore;

    [JsonProperty("mmrChange")] public readonly int MmrChange;

    [JsonConstructor]
    public MatchResultsPacket(MatchScore winner, MatchScore loser, int mmrChange)
    {
        WinnerScore = winner;
        LoserScore = loser;
        MmrChange = mmrChange;
    }
}