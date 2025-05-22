using System;
using HMUI;
using LoungeSaber.Models.Divisions;
using LoungeSaber.Server.MatchRoom;
using LoungeSaber.UI.BSML;
using LoungeSaber.UI.BSML.Match;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchRoomFlowCoordinator : FlowCoordinator, IInitializable, IDisposable
    {
        public event Action OnBackButtonPressed;

        [Inject] private readonly LoadingViewController _loadingViewController = null;
        [Inject] private readonly MatchWaitingRoomViewController _matchWaitingRoomViewController = null;
        
        [Inject] private readonly LoungeServerInterfacer _loungeServerInterfacer = null;
        
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetTitle("Connecting...");
            showBackButton = true;
            ProvideInitialViewControllers(_loadingViewController);
        }

        protected override void BackButtonWasPressed(ViewController _) => OnBackButtonPressed?.Invoke();
        
        public void Initialize()
        {
            _loungeServerInterfacer.OnConnected += OnConnected;
        }

        private void OnConnected()
        {
            SetTitle("Match Room");
            showBackButton = true;
            PresentViewController(_matchWaitingRoomViewController);
        }

        public void Dispose()
        {
            _loungeServerInterfacer.OnConnected += OnConnected;
        }
    }
}