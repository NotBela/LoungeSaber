using System.Globalization;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Models.Map;
using Zenject;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.WaitingForMatchToStartView.bsml")]
    public class WaitingForMatchToStartViewController : BSMLAutomaticViewController, ITickable
    {
        [UIValue("tempSongDisplayText")] private string _tempSongDisplayText { get; set; } = "";
        
        [UIValue("tempMatchStartTimer")] private string _tempMatchStartTimer { get; set; } = "";
        
        private DateTime _startTime = DateTime.UtcNow;
        
        public void PopulateData(VotingMap votingMap, DateTime startTime)
        {
            _tempSongDisplayText = votingMap.GetBeatmapLevel()?.songName;
            _startTime = startTime;
            
            NotifyPropertyChanged(null);
        }

        public void Tick()
        {
            if (!isActivated)
                return;
            
            _tempMatchStartTimer = $"Starting in {((int) (_startTime - DateTime.UtcNow).TotalSeconds).ToString(CultureInfo.InvariantCulture)}...";
            
            NotifyPropertyChanged(nameof(_tempMatchStartTimer));
        }
    }
}