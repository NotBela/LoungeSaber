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
        [Inject] private readonly InfoFlowCoordinator _infoFlowCoordinator = null;
        [Inject] private readonly VotingScreenViewController _votingScreenViewController = null;
        [Inject] private readonly MatchManager _matchManager = null;
        
        [Inject] private readonly ServerListener _serverListener = null;
        [Inject] private readonly MatchmakingMenuViewController _matchmakingMenuViewController = null;
        
        [Inject] private readonly MatchResultsViewController _matchResultsViewController = null;
        
        [Inject] private readonly LoungeSaberLeaderboardViewController _leaderboardViewController = null;

        [Inject] private readonly SiraLog _siraLog = null;
        
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            showBackButton = true;
            SetTitle("LoungeSaber");
            ProvideInitialViewControllers(_matchmakingMenuViewController, rightScreenViewController: _leaderboardViewController);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling) => showBackButton = true;

        private void OnContinueButtonPressed() => DismissFlowCoordinator(_matchFlowCoordinator);

        private void OnMatchCreated(MatchCreatedPacket packet)
        {
            try
            {
                _votingScreenViewController.didActivateEvent += OnActivated;
                
                PresentFlowCoordinatorSynchronously(_matchFlowCoordinator);

                void OnActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
                {
                    _votingScreenViewController.didActivateEvent -= OnActivated;
                    _matchManager.SetOpponent(packet.Opponent);
                    
                    _votingScreenViewController.PopulateData(packet);
                }
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        public void Dispose()
        {
            _serverListener.OnMatchCreated -= OnMatchCreated;
            _matchResultsViewController.ContinueButtonPressed -= OnContinueButtonPressed;
            _matchmakingMenuViewController.AboutButtonClicked -= OnAboutButtonClicked;
            _infoFlowCoordinator.OnBackButtonPressed -= OnInfoFlowCoordinatorBackButtonPressed;
        }
        
        public void Initialize()
        {
            _serverListener.OnMatchCreated += OnMatchCreated;
            _matchResultsViewController.ContinueButtonPressed += OnContinueButtonPressed;
            _matchmakingMenuViewController.AboutButtonClicked += OnAboutButtonClicked;
            _infoFlowCoordinator.OnBackButtonPressed += OnInfoFlowCoordinatorBackButtonPressed;
        }

        private void OnInfoFlowCoordinatorBackButtonPressed() => DismissFlowCoordinator(_infoFlowCoordinator);

        private void OnAboutButtonClicked()
        {
            PresentFlowCoordinatorSynchronously(_infoFlowCoordinator);
            _serverListener.Disconnect();
        }

        protected override void BackButtonWasPressed(ViewController _)
        {
            _serverListener.Disconnect();
            _mainFlowCoordinator.GetType().GetMethod("DismissChildFlowCoordinatorsRecursively", BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_mainFlowCoordinator,
                [false]);
        }
    }
}  