using System.Collections;
using System.Reflection;
using HarmonyLib;
using HMUI;
using IPA.Utilities;
using LoungeSaber.Game;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Server;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.Leaderboard;
using LoungeSaber.UI.BSML.Match;
using LoungeSaber.UI.BSML.Menu;
using SiraUtil.Logging;
using SongCore;
using Zenject;
using UnityEngine;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchmakingMenuFlowCoordinator : SynchronousFlowCoordinator, IInitializable, IDisposable
    {
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;
        [Inject] private readonly MatchFlowCoordinator _matchFlowCoordinator = null;
        [Inject] private readonly VotingScreenViewController _votingScreenViewController = null;
        
        [Inject] private readonly ServerListener _serverListener = null;
        [Inject] private readonly MatchmakingMenuViewController _matchmakingMenuViewController = null;
        
        [Inject] private readonly MatchResultsViewController _matchResultsViewController = null;
        
        [Inject] private readonly LoungeSaberLeaderboardViewController _leaderboardViewController = null;
        
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            showBackButton = true;
            SetTitle("LoungeSaber");
            ProvideInitialViewControllers(_matchmakingMenuViewController, rightScreenViewController: _leaderboardViewController);
        }

        private void OnContinueButtonPressed() => DismissFlowCoordinator(_matchFlowCoordinator);

        private void OnMatchCreated(MatchCreatedPacket packet)
        {
            PresentFlowCoordinatorSynchronously(_matchFlowCoordinator);
            _votingScreenViewController.PopulateData(packet);
        }

        public void Dispose()
        {
            _serverListener.OnMatchCreated -= OnMatchCreated;
            _matchResultsViewController.ContinueButtonPressed -= OnContinueButtonPressed;
        }
        
        public void Initialize()
        {
            _serverListener.OnMatchCreated += OnMatchCreated;
            _matchResultsViewController.ContinueButtonPressed += OnContinueButtonPressed;
        }

        protected override void BackButtonWasPressed(ViewController _)
        {
            // todo: look into replacing servercheckingflowcoordinator in flow coordinator hierarchy
        }
    }
}  