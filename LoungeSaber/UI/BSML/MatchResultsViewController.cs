using System;
using System.Drawing;
using System.Globalization;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using JetBrains.Annotations;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.UserPackets;
using Color = UnityEngine.Color;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.MatchResultsView.bsml")]
    public class MatchResultsViewController : BSMLAutomaticViewController
    {
        public event Action ContinueButtonPressed;
        
        [UIValue("titleBgColor")] private string TitleBgColor { get; set; } = "#0000FF";
        [UIValue("titleText")] private string TitleText { get; set; } = "You Win";
        
        [UIValue("clientNameText")] private string ClientNameText { get; set; } = "placeholder";
        [UIValue("clientScoreText")] private string ClientScoreText { get; set; } = "placeholder";
        [UIValue("newClientMmrText")] private string NewClientMmrText { get; set; } = "placeholder";
        
        [UIValue("opponentNameText")] private string OpponentNameText { get; set; } = "placeholder";
        [UIValue("opponentScoreText")] private string OpponentScoreText { get; set; } = "placeholder";
        [UIValue("newOpponentMmrText")] private string NewOpponentMmrText { get; set; } = "placeholder";
        
        public void PopulateData(MatchResults results)
        {
            TitleText = results.Winner == MatchResults.MatchWinner.You ? "You Win!" : "You Lose!";
            TitleBgColor = results.Winner == MatchResults.MatchWinner.You ? "#0000FF" : "#FF0000";

            ClientNameText = results.NewClientUserInfo.GetFormattedBadgeName();
            OpponentNameText = results.NewOpponentUserInfo.GetFormattedBadgeName();

            ClientScoreText = FormatScore(results.YourScore);
            OpponentScoreText = FormatScore(results.OpponentScore);

            NewOpponentMmrText = FormatMmrChange(results.MMRChange, results.NewOpponentUserInfo, results.Winner == MatchResults.MatchWinner.You);
            NewClientMmrText = FormatMmrChange(results.MMRChange, results.NewClientUserInfo, results.Winner == MatchResults.MatchWinner.Opponent);
            
            NotifyPropertyChanged(null);
        }

        private string FormatMmrChange(int change, Models.UserInfo.UserInfo info, bool won) => $"{info.Mmr} ({(!won ? "<color=#90EE90>+" : "<color=#FF7F7F>-")}{change}</color>)";

        private string FormatScore(ScoreSubmissionPacket scoreSubmission) => 
            $"{(scoreSubmission.Score / scoreSubmission.MaxScore * 100).ToString("F2", CultureInfo.InvariantCulture)}% " +
            $"({(scoreSubmission.FullCombo ? "<color=#90EE90>FC</color>" : $"<color=#FF7F7F>{scoreSubmission.MissCount}x</color>")})" +
            $"{(scoreSubmission.ProMode ? " [PM]" : "")}";
        
        [UIAction("continueButtonClicked")]
        private void ContinueButtonClicked() => ContinueButtonPressed?.Invoke();
    }
}