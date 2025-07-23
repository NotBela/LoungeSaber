using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using JetBrains.Annotations;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.AwaitingMapDecisionView.bsml")]
    public class AwaitingMapDecisionViewController : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        [Inject] private readonly ServerListener _serverListener = null;
        
        private List<VotingMap> _votingMaps = new();

        [UIValue("temporaryOpponentChoiceText")]
        public string TemporaryOpponentChoiceText { get; set; } = "Waiting...";
        
        [UIValue("temporaryPlayerChoiceText")]
        private string TemporaryPlayerChoiceText { get; set; } = "";
        
        public void PopulateData(VotingMap vote, List<VotingMap> votingMaps)
        {
            _votingMaps = votingMaps;
            TemporaryPlayerChoiceText = vote.GetBeatmapLevel()?.songName;
            
            NotifyPropertyChanged(null);
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            
            TemporaryOpponentChoiceText = TemporaryOpponentChoiceText;
        }

        private void OnOpponentVoted(OpponentVoted opponentVoted)
        {
            while (_votingMaps.Count == 0);
            
            TemporaryOpponentChoiceText = _votingMaps[opponentVoted.VoteIndex].GetBeatmapLevel()?.songName;
            
            NotifyPropertyChanged(null);
        }

        public void Dispose()
        {
            _serverListener.OnOpponentVoted -= OnOpponentVoted;
        }

        public void Initialize()
        {
            _serverListener.OnOpponentVoted += OnOpponentVoted;
        }
    }
}