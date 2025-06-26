using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using Zenject;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.AwaitingMapDecisionView.bsml")]
    public class AwaitingMapDecisionViewController : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        [Inject] private readonly ServerListener _serverListener = null;
        
        private List<VotingMap> _votingMaps = new List<VotingMap>();

        [UIValue("temporaryOpponentChoiceText")]
        public string TemporaryOpponentChoiceText { get; set; } = "Waiting...";
        
        [UIValue("temporaryPlayerChoiceText")]
        private string TemporaryPlayerChoiceText { get; set; } = "";
        
        public void PopulateData(List<VotingMap> votingMaps, VotingMap vote)
        {
            _votingMaps = votingMaps;
            TemporaryPlayerChoiceText = vote.GetBeatmapLevel()?.songName;
            
            NotifyPropertyChanged(null);
        }

        private void OnOpponentVoted(OpponentVoted opponentVoted)
        {
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