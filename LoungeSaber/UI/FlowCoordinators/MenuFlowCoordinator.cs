using BeatSaberMarkupLanguage;
using SiraUtil.Logging;
using HMUI;
using System;
using LoungeSaber.UI.BSML;
using Zenject;

namespace LoungeSaber.FlowCoordinators
{
    internal class MenuFlowCoordinator : FlowCoordinator
    {
        [Inject] private readonly SiraLog _siraLog = null;
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;

        [Inject] private readonly LoadingViewController _loadingViewController = null;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            try
            {
                if (firstActivation)
                {
                    SetTitle("LoungeSaber");
                    showBackButton = true;
                    ProvideInitialViewControllers(_loadingViewController);
                }
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
    }
}