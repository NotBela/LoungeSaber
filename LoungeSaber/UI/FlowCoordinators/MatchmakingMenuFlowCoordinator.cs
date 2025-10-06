using System.Collections;
using System.Reflection;
using HarmonyLib;
using HMUI;
using IPA.Utilities;
using LoungeSaber.Extensions;
using LoungeSaber.Game;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Server;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.Disconnect;
using LoungeSaber.UI.BSML.Leaderboard;
using LoungeSaber.UI.BSML.Match;
using LoungeSaber.UI.BSML.Menu;
using LoungeSaber.UI.FlowCoordinators.Events;
using SiraUtil.Logging;
using SongCore;
using Zenject;
using UnityEngine;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchmakingMenuFlowCoordinator : FlowCoordinator, IInitializable, IDisposable
    {
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;
        [Inject] private readonly MatchFlowCoordinator _matchFlowCoordinator = null;
        [Inject] private readonly InfoFlowCoordinator _infoFlowCoordinator = null;
        [Inject] private readonly VotingScreenViewController _votingScreenViewController = null;
        [Inject] private readonly MatchManager _matchManager = null;
        
        [Inject] private readonly IServerListener _serverListener = null;
        [Inject] private readonly MatchmakingMenuViewController _matchmakingMenuViewController = null;
        
        [Inject] private readonly MatchResultsViewController _matchResultsViewController = null;
        
        [Inject] private readonly LoungeSaberLeaderboardViewController _leaderboardViewController = null;

        [Inject] private readonly SiraLog _siraLog = null;
        
        
        [Inject] private readonly EventsFlowCoordinator _eventsFlowCoordinator = null;
        
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
                
                this.PresentFlowCoordinatorSynchronously(_matchFlowCoordinator);

                void OnActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
                {
                    _votingScreenViewController.didActivateEvent -= OnActivated;
                    _matchManager.Opponent = packet.Opponent;
                    
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
            _matchmakingMenuViewController.EventsButtonClicked += OnEventsButtonClicked;
        }
        
        public void Initialize()
        {
            _serverListener.OnMatchCreated += OnMatchCreated;
            _matchResultsViewController.ContinueButtonPressed += OnContinueButtonPressed;
            _matchmakingMenuViewController.AboutButtonClicked += OnAboutButtonClicked;
            _infoFlowCoordinator.OnBackButtonPressed += OnInfoFlowCoordinatorBackButtonPressed;
            _matchmakingMenuViewController.EventsButtonClicked += OnEventsButtonClicked;
            _eventsFlowCoordinator.OnBackButtonPressed += EventsFlowCoordinatorOnBackButtonPressed;
        }

        private void EventsFlowCoordinatorOnBackButtonPressed() => DismissFlowCoordinator(_eventsFlowCoordinator);

        private void OnEventsButtonClicked() => this.PresentFlowCoordinatorSynchronously(_eventsFlowCoordinator);

        private void OnInfoFlowCoordinatorBackButtonPressed() => DismissFlowCoordinator(_infoFlowCoordinator);

        private void OnAboutButtonClicked()
        {
            this.PresentFlowCoordinatorSynchronously(_infoFlowCoordinator);
        }

        protected override void BackButtonWasPressed(ViewController _)
        {
            _serverListener.Disconnect();
            _mainFlowCoordinator.DismissAllChildFlowCoordinators();
            
            // _mainFlowCoordinator.GetType().GetMethod("DismissChildFlowCoordinatorsRecursively", BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_mainFlowCoordinator,
            //     [false]);
        }
    }
}  