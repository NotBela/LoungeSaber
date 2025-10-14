using HMUI;
using LoungeSaber.Extensions;
using LoungeSaber.Game;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.ServerPackets.Event;
using LoungeSaber.UI.BSML.Events;
using LoungeSaber.UI.ViewManagers;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators.Events;

public class WaitingForEventMatchFlowCoordinator : FlowCoordinator
{
    [Inject] private readonly EventWaitingOnNextMatchViewController _waitingOnNextMatchViewController = null;
    [Inject] private readonly GameplaySetupViewManager _gameplaySetupViewManager = null;
    [Inject] private readonly EventMatchFlowCoordinator _eventMatchFlowCoordinator = null;
    
    [Inject] private readonly IServerListener _serverListener = null;
    
    public event Action OnBackButtonPressed; 
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        SetTitle("Event room");
        showBackButton = true;
        ProvideInitialViewControllers(_waitingOnNextMatchViewController, _gameplaySetupViewManager.ManagedController);
        
        _serverListener.OnMatchStarting += OnEventMatchStarted;
        _serverListener.OnEventStarted += OnEventStarted;
    }

    private void OnEventStarted(EventStartedPacket packet)
    {
        _waitingOnNextMatchViewController.SetText("Waiting for match to start...");
        
        
    }

    private void OnEventMatchStarted(MatchStarted packet)
    {
        this.PresentFlowCoordinatorSynchronously(_eventMatchFlowCoordinator);
        _eventMatchFlowCoordinator.Setup(packet, () =>
        {
            DismissFlowCoordinator(_eventMatchFlowCoordinator);
        });
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
        _serverListener.OnMatchStarting -= OnEventMatchStarted;
        _serverListener.OnEventStarted -= OnEventStarted;
    }

    protected override void BackButtonWasPressed(ViewController _)
    {
        _serverListener.Disconnect();
        OnBackButtonPressed?.Invoke();
    }
}