using System;
using HMUI;
using JetBrains.Annotations;
using LoungeSaber.Managers;
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

        [Inject] private readonly StateManager _stateManager = null;
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        
        [Inject] private readonly LoadingViewController _loadingViewController = null;
        [Inject] private readonly DivisionSelectorViewController _divisionSelectorViewController = null;
        
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            try
            {
                if (!firstActivation) 
                    return;
                
                SetTitle("LoungeSaber");
                showBackButton = true;
                ProvideInitialViewControllers(_loadingViewController);
                
                _stateManager.SwitchState(StateManager.State.DivisionSelector);
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
            _stateManager.StateChanged += OnStateChanged;
        }

        private void OnStateChanged(StateManager.State state)
        {
            try
            {
                switch (state)
                {
                    case StateManager.State.Loading:
                        SetViewControllers("LoungeSaber", true, _loadingViewController);
                        break;
                    case StateManager.State.DivisionSelector:
                        SetViewControllers("LoungeSaber Divisions", true, _divisionSelectorViewController);
                        break;
                }
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        public void Dispose()
        {
            _stateManager.StateChanged -= OnStateChanged;
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