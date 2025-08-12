using Newtonsoft.Json;

namespace LoungeSaber.Models.UserInfo;

[method: JsonConstructor]
public class DivisionInfo(DivisionInfo.DivisionName division, byte subDivision, string colorCode)
{
    [JsonProperty("division")] public DivisionName Division { get; private set; } = division;
    [JsonProperty("subDivision")] public byte SubDivision { get; private set; } = subDivision;
    [JsonProperty("color")] public string Color { get; private set; } = colorCode;

    public enum DivisionName
    {
        Iron,
        Bronze,
        Silver,
        Gold,
        Diamond,
        Platinum,
        Master,
        GrandMaster
    }
}