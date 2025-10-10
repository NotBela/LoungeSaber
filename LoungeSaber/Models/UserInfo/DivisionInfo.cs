using Newtonsoft.Json;

namespace LoungeSaber.Models.UserInfo;

[method: JsonConstructor]
public class DivisionInfo(string division, byte subDivision, string colorCode)
{
    [JsonProperty("division")] public string Division { get; private set; } = division;
    [JsonProperty("subDivision")] public byte SubDivision { get; private set; } = subDivision;
    [JsonProperty("color")] public string Color { get; private set; } = colorCode;
}