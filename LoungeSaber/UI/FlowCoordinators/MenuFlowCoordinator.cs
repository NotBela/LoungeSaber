using System;
using HMUI;
using JetBrains.Annotations;
using LoungeSaber.Server.Api;
using LoungeSaber.UI.BSML;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators
{
    internal class MenuFlowCoordinator : FlowCoordinator, IInitializable, IDisposable
    {
        [Inject] private readonly SiraLog _siraLog = null;
        
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;

        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        
        [Inject] private readonly DivisionSelectorViewController _divisionSelectorViewController = null;
        
        protected override async void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            try
            {
                if (!firstActivation) 
                    return;
                
                SetTitle("LoungeSaber");
                showBackButton = true;
                ProvideInitialViewControllers(_divisionSelectorViewController);
                
                await _loungeSaberApi.RequestDivisionDataRefresh();
            }
            catch (Exception ex)
            {
                _siraLog.Error(ex);
            }
        }

        protected override void BackButtonWasPressed(ViewController _)
        {
            _mainFlowCoordinator.DismissFlowCoordinator(this);
        }

        public void Initialize()
        {
            _loungeSaberApi.OnDivisionDataRefreshStarted += OnDivisionDataRefreshed;
        }

        private void OnDivisionDataRefreshed() => SetViewControllers("LoungeSaber Divisions", true, _divisionSelectorViewController);

        public void Dispose()
        {
            _loungeSaberApi.OnDivisionDataRefreshStarted -= OnDivisionDataRefreshed;
        }

        private void OnConnectStarted()
        {
            
        }

        private void OnMatchRoomBackButtonPressed()
        {
            
        }

        private void SetViewControllers(string newTitle, bool backButtonVisible, ViewController center,
            [CanBeNull] ViewController left = null, [CanBeNull] ViewController right = null)
        {
            SetTitle(newTitle);
            showBackButton = backButtonVisible;
            
            PresentViewController(center);
            SetLeftScreenViewController(left, ViewController.AnimationType.In);
            SetRightScreenViewController(right, ViewController.AnimationType.In);
        }
    }
}