﻿using Newtonsoft.Json;

namespace LoungeSaber.Models.UserInfo;

[method: JsonConstructor]
public class UserInfo(string username, string userId, int mmr, Badge? badge, long rank, string? discordId, bool banned, DivisionInfo divisionInfo)
{
    [JsonProperty("username")]
    public string Username { get; private set; } = username;

    [JsonProperty("userId")]
    public string UserId { get; private set; } = userId;

    [JsonProperty("mmr")]
    public int Mmr { get; private set; }= mmr;

    [JsonProperty("badge")]
    public Badge? Badge { get; private set; }= badge;

    [JsonProperty("rank")]
    public long Rank { get; private set; }= rank;

    [JsonProperty("discordId")]
    public string? DiscordId { get; private set; } = discordId;

    [JsonProperty("banned")]
    public bool Banned { get; private set; } = banned;
    
    [JsonProperty("division")]
    public DivisionInfo Division { get; private set; } = divisionInfo;
}