using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using HarmonyLib;
using HMUI;
using IPA.Utilities.Async;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML;
using LoungeSaber.UI.BSML.Match;
using SiraUtil.Logging;
using Zenject;
using UnityEngine;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchmakingMenuFlowCoordinator : FlowCoordinator, IInitializable, IDisposable
    {
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;
        [Inject] private readonly MatchFlowCoordinator _matchFlowCoordinator = null;
        [Inject] private readonly VotingScreenViewController _votingScreenViewController = null;
        
        [Inject] private readonly ServerListener _serverListener = null;
        [Inject] private readonly MatchmakingMenuViewController _matchmakingMenuViewController = null;
        
        [Inject] private readonly MatchResultsViewController _matchResultsViewController = null;
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            showBackButton = true;
            ProvideInitialViewControllers(_matchmakingMenuViewController);
            SetTitle("LoungeSaber");
        }

        private void OnContinueButtonPressed() => DismissFlowCoordinator(_matchFlowCoordinator);

        public void Initialize()
        {
            _serverListener.OnMatchCreated += OnMatchCreated;
            _matchResultsViewController.ContinueButtonPressed += OnContinueButtonPressed;
        }

        private void OnMatchCreated(MatchCreatedPacket packet)
        {
            StartCoroutine(PresentViewControllerSynchronously(packet));
        }

        IEnumerator PresentViewControllerSynchronously(MatchCreatedPacket packet)
        {
            yield return new WaitForEndOfFrame();
            
            PresentFlowCoordinator(_matchFlowCoordinator);
            _votingScreenViewController.PopulateData(packet);
        }

        public void Dispose()
        {
            _serverListener.OnMatchCreated -= OnMatchCreated;
            _matchResultsViewController.ContinueButtonPressed -= OnContinueButtonPressed;
        }

        protected override void BackButtonWasPressed(ViewController _)
        {
            _mainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}  