using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Models.Map;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.WaitingForMatchToStartView.bsml")]
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