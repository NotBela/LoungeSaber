using System.Linq;
using HarmonyLib;
using HMUI;
using IPA.Utilities.Async;
using LoungeSaber.UI.BSML;
using SiraUtil.Logging;
using Zenject;
using UnityEngine;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchmakingMenuFlowCoordinator : FlowCoordinator
    {
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;
        
        [Inject] private readonly MatchmakingMenuViewController _matchmakingMenuViewController = null;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            showBackButton = true;
            ProvideInitialViewControllers(_matchmakingMenuViewController);
            SetTitle("LoungeSaber");
        }

        protected override void BackButtonWasPressed(ViewController _)
        {
            _mainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}  