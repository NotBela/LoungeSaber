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

        private LevelBar _ownVoteLevelBar;
        private LevelBar _opponentVoteLevelBar;
        
        public void PopulateData(VotingMap vote, List<VotingMap> votingMaps)
        {
            _votingMaps = votingMaps;
            
            _ownVoteLevelBar.Setup(vote.GetBeatmapKey());

            TemporaryPlayerChoiceText = vote.GetBeatmapLevel()?.songName;
            
            NotifyPropertyChanged(null);
        }

        private void Awake()
        {
            _ownVoteLevelBar = CreateLevelBar(new Vector3(0, -10, 0));
            _opponentVoteLevelBar = CreateLevelBar(new Vector3(0, 10, 0));
        }

        private void OnOpponentVoted(OpponentVoted opponentVoted)
        {
            while (_votingMaps.Count == 0);

            _opponentVoteLevelBar.Setup(_votingMaps[opponentVoted.VoteIndex].GetBeatmapKey());
        }

        private LevelBar CreateLevelBar(Vector3 position)
        {
            var levelBar = Instantiate(_standardLevelDetailViewController._standardLevelDetailView._levelBar, transform, false);
            levelBar.gameObject.SetActive(true);
            levelBar.transform.localPosition = position;
            // levelBar.transform.localScale = new Vector3(0.5f, .5f, levelBar.transform.localScale.z);
            
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