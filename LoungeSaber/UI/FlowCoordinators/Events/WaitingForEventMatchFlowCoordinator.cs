using HMUI;
using LoungeSaber.Game;
using LoungeSaber.Interfaces;
using LoungeSaber.UI.BSML.Events;
using LoungeSaber.UI.ViewManagers;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators.Events;

public class WaitingForEventMatchFlowCoordinator : FlowCoordinator
{
    [Inject] private readonly EventWaitingOnNextMatchViewController _waitingOnNextMatchViewController = null;
    [Inject] private readonly GameplaySetupViewManager _gameplaySetupViewManager = null;
    
    [Inject] private readonly IServerListener _serverListener = null;
    
    public event Action OnBackButtonPressed; 
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        SetTitle("Event room");
        showBackButton = true;
        ProvideInitialViewControllers(_waitingOnNextMatchViewController, _gameplaySetupViewManager.ManagedController);
    }

    protected override void BackButtonWasPressed(ViewController _)
    {
        _serverListener.Disconnect();
        OnBackButtonPressed?.Invoke();
    }
}