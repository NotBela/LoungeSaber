using System.Globalization;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Extensions;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.ServerPackets.Match;
using LoungeSaber.Models.Packets.UserPackets;
using Zenject;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.MatchResultsView.bsml")]
    public class MatchResultsViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly IPlatformUserModel _platformUserModel = null;
        
        public event Action ContinueButtonPressed;
        
        [UIValue("titleBgColor")] private string TitleBgColor { get; set; } = "#0000FF";
        [UIValue("titleText")] private string TitleText { get; set; } = "You Win";
        
        [UIValue("clientNameText")] private string ClientNameText { get; set; } = "placeholder";
        [UIValue("clientScoreText")] private string ClientScoreText { get; set; } = "placeholder";
        [UIValue("newClientMmrText")] private string NewClientMmrText { get; set; } = "placeholder";
        
        [UIValue("opponentNameText")] private string OpponentNameText { get; set; } = "placeholder";
        [UIValue("opponentScoreText")] private string OpponentScoreText { get; set; } = "placeholder";
        [UIValue("newOpponentMmrText")] private string NewOpponentMmrText { get; set; } = "placeholder";
        
        public void PopulateData(MatchResultsPacket results)
        {
            var won = results.WinnerScore.User.UserId ==
                      _platformUserModel.GetUserInfo(CancellationToken.None).Result.platformUserId;
            
            TitleText = won ? "You Win!" : "You Lose!";
            TitleBgColor = won ? "#0000FF" : "#FF0000";

            ClientNameText = won ? results.WinnerScore.User.GetFormattedUserName() : results.LoserScore.User.GetFormattedUserName();
            OpponentNameText = won ? results.LoserScore.User.GetFormattedUserName() : results.WinnerScore.User.GetFormattedUserName();

            ClientScoreText = FormatScore(won ? results.WinnerScore.Score : results.LoserScore.Score);
            OpponentScoreText = FormatScore(won ? results.LoserScore.Score : results.WinnerScore.Score);

            NewOpponentMmrText = results.MmrChange.ToString().FormatWithHtmlColor(!won ? "#90EE90" : "#FF7F7F");
            NewClientMmrText = results.MmrChange.ToString().FormatWithHtmlColor(!won ? "#90EE90" : "#FF7F7F");
            
            NotifyPropertyChanged(null);
        }

        // private string FormatMmrChange(int change, Models.UserInfo.UserInfo info, bool won) => $"{info.Mmr} ({(!won ? "<color=#90EE90>+" : "<color=#FF7F7F>-")}{change}</color>)";

        private string FormatScore(Score scoreSubmission) => 
            $"{(scoreSubmission.RelativeScore).ToString("F2", CultureInfo.InvariantCulture)}% " +
            $"({(scoreSubmission.FullCombo ? "<color=#90EE90>FC</color>" : $"<color=#FF7F7F>{scoreSubmission.Misses}x</color>")})" +
            $"{(scoreSubmission.ProMode ? " [PM]" : "")}";
        
        [UIAction("continueButtonClicked")]
        private void ContinueButtonClicked() => ContinueButtonPressed?.Invoke();
    }
}