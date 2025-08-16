using BeatSaberMarkupLanguage.Attributes;
using LoungeSaber.Extensions;

namespace LoungeSaber.UI.BSML.Components;

public class LeaderboardSlot(Models.UserInfo.UserInfo userInfo, bool isSelf)
{
    private const string OwnCellTextColor = "#00C0FF";

    public event Action<Models.UserInfo.UserInfo> OnUserInfoButtonClicked;

    [UIValue("leaderboardCellColor")] private string LeaderboardCellColor { get; set; } = isSelf ? OwnCellTextColor : "white";

    [UIValue("rankText")] private string RankText { get; set; } = userInfo.Rank + ".";
    [UIValue("playerNameText")] private string NameText { get; set; } = userInfo.GetFormattedUserName();
    [UIValue("mmrText")] private string MmrText { get; set; } = $"{userInfo.Mmr:N0} MMR".FormatWithHtmlColor(isSelf ? OwnCellTextColor : userInfo.Division.Color);

    [UIAction("profileButtonOnClick")]
    private void ProfileButtonOnClick() => OnUserInfoButtonClicked?.Invoke(userInfo);
}