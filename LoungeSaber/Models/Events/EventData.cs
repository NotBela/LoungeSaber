using Newtonsoft.Json;

namespace LoungeSaber.Models.Events;

[method: Newtonsoft.Json.JsonConstructor]
public class EventData(string eventName, string displayName, string description)
{
    [JsonProperty(PropertyName = "eventName")]
    public string Name => eventName;
    
    [JsonProperty(PropertyName = "displayName")]
    public string DisplayName => displayName;
    
    [JsonProperty(PropertyName = "description")]
    public string Description => description;
}