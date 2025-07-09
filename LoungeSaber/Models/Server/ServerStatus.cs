using Newtonsoft.Json;

namespace LoungeSaber.Models.Server;

public class ServerStatus
{
    [JsonProperty("allowedGameVersions")]
    public readonly string[] AllowedGameVersions;
    
    [JsonProperty("allowedModVersions")]
    public readonly string[] AllowedModVersions;
    
    [JsonProperty("state")]
    public readonly ServerState State;

    [JsonConstructor]
    public ServerStatus(string[] allowedGameVersions, string[] allowedModVersions, ServerState state)
    {
        AllowedGameVersions = allowedGameVersions;
        AllowedModVersions = allowedModVersions;
        State = state;
    }

    public enum ServerState
    {
        Online,
        Maintenance
    }
}