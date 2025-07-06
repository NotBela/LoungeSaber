using BeatSaberMarkupLanguage.Attributes;

namespace LoungeSaber.UI.BSML.Components;

public class LeaderboardSlot
{
    [UIValue("rankText")] private string _rankText { get; set; }
    [UIValue("playerNameText")] private string _nameText { get; set; }
    [UIValue("mmrText")] private string _mmrText { get; set; }
    
    public LeaderboardSlot(Models.UserInfo.UserInfo userInfo)
    {
        _rankText = userInfo.Rank.ToString();
        _nameText = userInfo.GetFormattedUserName();
        _mmrText = $"<color=#311B92>{userInfo.Mmr} MMR</color>";
    }
}