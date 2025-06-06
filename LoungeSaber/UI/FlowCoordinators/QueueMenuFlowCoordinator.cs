using System.Linq;
using HarmonyLib;
using HMUI;
using IPA.Utilities.Async;
using SiraUtil.Logging;
using Zenject;
using UnityEngine;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class QueueMenuFlowCoordinator : FlowCoordinator
    {
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;
        [Inject] private readonly DiContainer _diContainer = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        [Inject] private readonly GameplaySetupViewController _alternateGameplaySetupViewController = null;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetupGameplaySetupViewController();
            
            showBackButton = true;
            ProvideInitialViewControllers(_alternateGameplaySetupViewController);
            SetTitle("LoungeSaber");
        }

        private void SetupGameplaySetupViewController()
        {
            _alternateGameplaySetupViewController.didActivateEvent += OnGameplaySetupViewActivated;
            _alternateGameplaySetupViewController.didDeactivateEvent += OnGameplaySetupViewDeactivated;
            
            _alternateGameplaySetupViewController.Setup(true, true,true, false, PlayerSettingsPanelController.PlayerSettingsPanelLayout.Singleplayer);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            _alternateGameplaySetupViewController.didActivateEvent -= OnGameplaySetupViewActivated;
            _alternateGameplaySetupViewController.didDeactivateEvent -= OnGameplaySetupViewDeactivated;
        }

        private void OnGameplaySetupViewDeactivated(bool removedFromHierarchy, bool screenSystemDisabling) => _alternateGameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles.Do(i => i.gameObject.SetActive(true));

        private void OnGameplaySetupViewActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) => _alternateGameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles.Where(i => i.name != "ProMode").Do(i => i.gameObject.SetActive(false));

        protected override void BackButtonWasPressed(ViewController _) => _mainFlowCoordinator.DismissFlowCoordinator(this);
    }
}  