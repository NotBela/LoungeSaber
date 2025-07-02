using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Models.Map;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.WaitingForMatchToStartView.bsml")]
    public class WaitingForMatchToStartViewController : BSMLAutomaticViewController
    {
        [UIValue("tempSongDisplayText")] private string _tempSongDisplayText { get; set; } = "";
        
        public void PopulateData(VotingMap votingMap)
        {
            _tempSongDisplayText = votingMap.GetBeatmapLevel()?.songName;
            
            NotifyPropertyChanged(null);
        }
    }
}