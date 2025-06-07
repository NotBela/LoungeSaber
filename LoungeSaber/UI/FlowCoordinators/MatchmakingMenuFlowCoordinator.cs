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
        [Inject] private readonly DiContainer _diContainer = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        [Inject] private readonly GameplaySetupViewController _gameplaySetupViewController = null;
        [Inject] private readonly MatchmakingMenuViewController _matchmakingMenuViewController = null;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetupGameplaySetupViewController();
            
            showBackButton = true;
            ProvideInitialViewControllers(_matchmakingMenuViewController, _gameplaySetupViewController);
            SetTitle("LoungeSaber");
        }

        private void SetupGameplaySetupViewController()
        {
            _gameplaySetupViewController.didActivateEvent += OnGameplaySetupViewActivated;
            
            _gameplaySetupViewController.Setup(true, true,true, false, PlayerSettingsPanelController.PlayerSettingsPanelLayout.Singleplayer);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            _gameplaySetupViewController.didActivateEvent -= OnGameplaySetupViewActivated;
        }

        private void ResetGameplaySetupView()
        {
            _gameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles.Do(i =>
                i.gameObject.SetActive(true));
        }

        private void OnGameplaySetupViewActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) => _gameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles.Where(i => i.name != "ProMode").Do(i => i.gameObject.SetActive(false));

        protected override void BackButtonWasPressed(ViewController _)
        {
            _mainFlowCoordinator.DismissFlowCoordinator(this);
            
            ResetGameplaySetupView();
        }
    }
}  