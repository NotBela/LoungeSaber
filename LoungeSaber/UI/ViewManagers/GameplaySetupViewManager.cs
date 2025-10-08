﻿using HarmonyLib;
using HMUI;
using LoungeSaber.UI.FlowCoordinators;
using LoungeSaber.UI.FlowCoordinators.Events;
using Zenject;

namespace LoungeSaber.UI.ViewManagers;

public class GameplaySetupViewManager : ViewManager
{
    [Inject] private readonly GameplaySetupViewController _gameplaySetupViewController = null;
    
    public override ViewController ManagedController => _gameplaySetupViewController;

    public bool ProMode => _gameplaySetupViewController.gameplayModifiers.proMode;

    protected override void SetupManagedController(FlowCoordinator parentFlowCoordinator)
    {
        _gameplaySetupViewController.Setup(true, true,true, false, PlayerSettingsPanelController.PlayerSettingsPanelLayout.Singleplayer);
        
        _gameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles
            .Where(i => i.name != "ProMode")
            .Do(i => i.gameObject.SetActive(false));
    }

    protected override void ResetManagedController()
    {
        _gameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles.Do(i => i.gameObject.SetActive(true));
    }
}