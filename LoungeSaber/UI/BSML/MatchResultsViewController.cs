using System.Drawing;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Models.Packets.ServerPackets;
using Color = UnityEngine.Color;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.MatchResultsView.bsml")]
    public class MatchResultsViewController : BSMLAutomaticViewController
    {
        [UIValue("titleBgColor")] private string _titleBgColor { get; set; } = "#0000FF";
        
        [UIValue("titleText")] private string _titleText { get; set; } = "You Win";
        
        public void PopulateData(MatchResults results)
        {
            _titleText = results.Winner == MatchResults.MatchWinner.You ? "You Win!" : "You Lose!";
            _titleBgColor = results.Winner == MatchResults.MatchWinner.You ? "#0000FF" : "#FF0000";
            
            NotifyPropertyChanged(null);
        }
    }
}