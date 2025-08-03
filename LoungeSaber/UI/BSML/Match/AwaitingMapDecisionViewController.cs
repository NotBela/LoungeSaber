using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using JetBrains.Annotations;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using SiraUtil.Logging;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.AwaitingMapDecisionView.bsml")]
    public class AwaitingMapDecisionViewController : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        [Inject] private readonly ServerListener _serverListener = null;

        [Inject] private readonly StandardLevelDetailViewController _standardLevelDetailViewController = null;
        
        private List<VotingMap> _votingMaps = new();

        [UIValue("temporaryOpponentChoiceText")]
        public string TemporaryOpponentChoiceText { get; set; } = "Waiting...";
        
        [UIValue("temporaryPlayerChoiceText")]
        private string TemporaryPlayerChoiceText { get; set; } = "";

        [UIComponent("ownLevelBar")] private readonly LevelBar _ownVoteLevelBar = null;
        [UIComponent("opponentLevelBar")] private LevelBar _opponentVoteLevelBar;

        [UIObject("opponentVoteContainer")] private readonly GameObject _opponentVoteContainer = null;
        [UIObject("ownVoteContainer")] private readonly GameObject _ownVoteContainer = null;
        
        public void PopulateData(VotingMap vote, List<VotingMap> votingMaps)
        {
            _votingMaps = votingMaps;
            
            _ownVoteLevelBar.Setup(vote.GetBeatmapKey());

            TemporaryPlayerChoiceText = vote.GetBeatmapLevel()?.songName;
            
            NotifyPropertyChanged(null);
        }

        [UIAction("#post-parse")]
        private void PostParse()
        {
            
        }

        private void OnOpponentVoted(OpponentVoted opponentVoted)
        {
            while (_votingMaps.Count == 0);

            _opponentVoteLevelBar.Setup(_votingMaps[opponentVoted.VoteIndex].GetBeatmapKey());
        }

        private LevelBar CreateLevelBar(GameObject parent)
        {
            var levelBar = Instantiate(_standardLevelDetailViewController._standardLevelDetailView._levelBar, gameObject.transform, false);
            levelBar.gameObject.SetActive(true);
            
            levelBar._showDifficultyAndCharacteristic = true;
            
            return levelBar;
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