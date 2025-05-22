using System;
using System.Threading.Tasks;
using HMUI;
using LoungeSaber.Models.Divisions;
using LoungeSaber.Server.MatchRoom;
using LoungeSaber.UI.BSML;
using LoungeSaber.UI.BSML.Match;
using SiraUtil.Logging;
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
        
        [Inject] private readonly SiraLog _siraLog = null;
        
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (!firstActivation) return;
            
            SetTitle("Connecting...");
            showBackButton = true;
            ProvideInitialViewControllers(_loadingViewController);
        }

        protected override async void BackButtonWasPressed(ViewController _)
        {
            try
            {
                await _loungeServerInterfacer.DisconnectFromLoungeServer();
                OnBackButtonPressed?.Invoke();
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        public void Initialize()
        {
            _loungeServerInterfacer.OnConnected += OnConnected;
            _loungeServerInterfacer.OnDisconnectByServer += OnDisconnectByServer;
        }

        private void OnDisconnectByServer(string reason)
        {
            
        }

        private async void OnConnected()
        {
            try
            {
                // put this here or the view controller wont present
                await Task.Delay(500);
            
                SetTitle("Match Room");
                showBackButton = true;
                PresentViewController(_matchWaitingRoomViewController);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        private async Task SetViewControllers()
        {
            await Task.Delay(500);
            
            
        }

        public void Dispose()
        {
            _loungeServerInterfacer.OnConnected += OnConnected;
        }
    }
}