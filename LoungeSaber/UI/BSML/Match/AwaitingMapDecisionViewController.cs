using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using JetBrains.Annotations;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using SiraUtil.Logging;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.AwaitingMapDecisionView.bsml")]
    public class AwaitingMapDecisionViewController : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        [Inject] private readonly ServerListener _serverListener = null;
        
        private List<VotingMap> _votingMaps = new();

        [UIValue("ownVoteHash")] private string OwnVoteHash { get; set; } = "";
        [UIValue("ownVoteDifficulty")] private string OwnVoteDifficulty { get; set; } = "yeah";

        [UIValue("opponentVoteHash")] private string OpponentVoteHash { get; set; } = "";
        [UIValue("opponentVoteDifficulty")] private string OpponentVoteDifficulty { get; set; }
        
        public void PopulateData(VotingMap vote, List<VotingMap> votingMaps)
        {
            _votingMaps = votingMaps;

            OwnVoteHash = vote.Hash;
            OwnVoteDifficulty = vote.GetBaseGameDifficultyType().Name();
            
            NotifyPropertyChanged(null);
        }

        private void OnOpponentVoted(OpponentVoted opponentVoted)
        {
            while (_votingMaps.Count == 0);

            OpponentVoteHash = _votingMaps[opponentVoted.VoteIndex].Hash;
            OpponentVoteDifficulty = _votingMaps[opponentVoted.VoteIndex].GetBaseGameDifficultyType().Name();
            
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