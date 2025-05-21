using System;
using HMUI;
using LoungeSaber.Models.Divisions;
using LoungeSaber.UI.BSML;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchRoomFlowCoordinator : FlowCoordinator
    {
        public event Action OnBackButtonPressed; 
        
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetTitle("Connecting...");
            showBackButton = true;
            ProvideInitialViewControllers(new GameObject().AddComponent<LoadingViewController>());
        }

        protected override void BackButtonWasPressed(ViewController _) => OnBackButtonPressed?.Invoke();
    }
}