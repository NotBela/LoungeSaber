using System;
using System.Collections;
using System.Linq;
using BeatSaberMarkupLanguage.GameplaySetup;
using HarmonyLib;
using HMUI;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchFlowCoordinator : FlowCoordinator
    {
        [Inject] private readonly GameplaySetupViewController _gameplaySetupViewController = null;
        [Inject] private readonly VotingScreenViewController _votingScreenViewController = null;
        
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetupGameplaySetupViewController();
            
            SetTitle("Match Room");
            showBackButton = true;
            ProvideInitialViewControllers(_votingScreenViewController, _gameplaySetupViewController);
        }
        
        private void SetupGameplaySetupViewController()
        {
            _gameplaySetupViewController.didActivateEvent += OnGameplaySetupViewActivated;
            
            _gameplaySetupViewController.Setup(true, true,true, false, PlayerSettingsPanelController.PlayerSettingsPanelLayout.Singleplayer);
        }
        
        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            ResetGameplaySetupView();
            
            _gameplaySetupViewController.didActivateEvent -= OnGameplaySetupViewActivated;
        }
        
        private void ResetGameplaySetupView() => _gameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles.Do(i => i.gameObject.SetActive(true));

        private void OnGameplaySetupViewActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) => 
            _gameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles
            .Where(i => i.name != "ProMode")
            .Do(i => i.gameObject.SetActive(false));
    }
}