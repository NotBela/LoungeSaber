using BeatSaberMarkupLanguage;
using HMUI;
using LoungeSaber.Game;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.UserPackets;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.Match;
using LoungeSaber.UI.ViewManagers;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchFlowCoordinator : SynchronousFlowCoordinator
    {
        [Inject] private readonly VotingScreenViewController _votingScreenViewController = null;
        [Inject] private readonly AwaitingMapDecisionViewController _awaitingMapDecisionViewController = null;
        [Inject] private readonly WaitingForMatchToStartViewController _waitingForMatchToStartViewController = null;
        [Inject] private readonly AwaitMatchEndViewController _awaitMatchEndViewController = null;
        [Inject] private readonly MatchResultsViewController _matchResultsViewController = null;
        
        [Inject] private readonly ServerListener _serverListener = null;
        [Inject] private readonly MatchManager _matchManager = null;
        
        [Inject] private readonly SiraLog _siraLog = null;
        
        [Inject] private readonly StandardLevelDetailViewManager _standardLevelDetailViewManager = null;
        [Inject] private readonly GameplaySetupViewManager _gameplaySetupViewManager = null;

        private NavigationController _votingScreenNavigationController;
         
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            ViewManager.Active = true;
            
            SetTitle("Match Room");
            showBackButton = false;
            
            _votingScreenNavigationController = BeatSaberUI.CreateViewController<NavigationController>();
            
            ProvideInitialViewControllers(_votingScreenNavigationController, _gameplaySetupViewManager.ManagedController);
            _votingScreenNavigationController.PushViewController(_votingScreenViewController, null);

            _votingScreenViewController.MapSelected += OnVotingMapSelected;
            _serverListener.OnMatchStarting += OnMatchStarting;
            _matchManager.OnLevelCompleted += OnLevelCompleted;
            _serverListener.OnMatchResults += OnMatchResultsReceived;
            _standardLevelDetailViewManager.OnMapVoteButtonPressed += OnMapVotedFor;
        }

        private void OnVotingMapSelected(VotingMap votingMap, List<VotingMap> votingMaps)
        {
            // todo: fix voting screen view controller being wide
            if (!_standardLevelDetailViewManager.ManagedController.isActivated)
                _votingScreenNavigationController.PushViewController(_standardLevelDetailViewManager.ManagedController, () => {});
            
            _standardLevelDetailViewManager.SetData(votingMap, votingMaps);
        }

        private void OnMatchResultsReceived(MatchResults results)
        {
            ReplaceViewControllerSynchronously(_matchResultsViewController);
            _matchResultsViewController.PopulateData(results);
        }

        private void OnLevelCompleted(LevelCompletionResults levelCompletionResults, StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupData)
        {
            Task.Run(async () =>
            {
                await _serverListener.SendPacket(new ScoreSubmissionPacket(levelCompletionResults.multipliedScore, ScoreModel.ComputeMaxMultipliedScoreForBeatmap(standardLevelScenesTransitionSetupData.transformedBeatmapData),
                    levelCompletionResults.gameplayModifiers.proMode, levelCompletionResults.notGoodCount, levelCompletionResults.fullCombo));
            });
            ReplaceViewControllerSynchronously(_awaitMatchEndViewController, immediately: true);
        }

        private async void OnMatchStarting(MatchStarted packet)
        {
            try
            {
                ReplaceViewControllerSynchronously(_waitingForMatchToStartViewController);
                _waitingForMatchToStartViewController.PopulateData(packet.MapSelected);

                await Task.Delay(packet.TransitionToGameTime - DateTime.UtcNow);
                _matchManager.StartMatch(packet.MapSelected, packet.StartingTime, _gameplaySetupViewManager.ProMode);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        private async void OnMapVotedFor(VotingMap votingMap, List<VotingMap> votingMaps)
        {
            try
            {
                _votingScreenNavigationController.PopViewControllers(1, () => {}, true);
                
                ReplaceTopViewController(_awaitingMapDecisionViewController);
                _awaitingMapDecisionViewController.PopulateData(votingMap, votingMaps);

                await _serverListener.SendPacket(new VotePacket(votingMaps.IndexOf(votingMap)));
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
        
        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            ViewManager.Active = false;

            _votingScreenViewController.MapSelected -= OnVotingMapSelected;
            _serverListener.OnMatchStarting -= OnMatchStarting;
            _matchManager.OnLevelCompleted -= OnLevelCompleted;
            _serverListener.OnMatchResults -= OnMatchResultsReceived;
            _standardLevelDetailViewManager.OnMapVoteButtonPressed -= OnMapVotedFor;
        }
    }
}