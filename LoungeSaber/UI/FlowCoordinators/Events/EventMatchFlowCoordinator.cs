using System.Text.RegularExpressions;
using HMUI;
using JetBrains.Annotations;
using LoungeSaber.UI.BSML.Match;
using LoungeSaber.UI.ViewManagers;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators.Events;

public class EventMatchFlowCoordinator : FlowCoordinator
{
    [Inject] private readonly WaitingForMatchToStartViewController _waitingForMatchToStartViewController = null;
    [Inject] private readonly GameplaySetupViewManager _gameplaySetupViewManager = null;
    [Inject] private readonly OpponentViewController _opponentViewController = null;
    
    [CanBeNull] public event Action OnMatchEnded;
     
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        SetTitle("Match Room");
        showBackButton = false;
        
        ProvideInitialViewControllers(_waitingForMatchToStartViewController, _gameplaySetupViewManager.ManagedController, bottomScreenViewController: _opponentViewController);
    }

    public void Setup()
    {
        
    }
}