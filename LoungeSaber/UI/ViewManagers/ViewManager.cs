using BeatSaberMarkupLanguage;
using HMUI;
using LoungeSaber.UI.FlowCoordinators;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.ViewManagers;

public abstract class ViewManager : MonoBehaviour, IInitializable, IDisposable
{
    public abstract ViewController ManagedController { get; }
    
    [Inject] private readonly MatchFlowCoordinator _matchFlowCoordinator = null;
    [Inject] private readonly MatchmakingMenuFlowCoordinator _matchmakingMenuFlowCoordinator = null;

    public void Initialize()
    {
        ManagedController.didActivateEvent += OnManagedControllerActivated;
        ManagedController.didDeactivateEvent += OnManagedControllerDeactivated;
    }

    public void Dispose()
    {
        ManagedController.didActivateEvent -= OnManagedControllerActivated;
        ManagedController.didDeactivateEvent -= OnManagedControllerDeactivated;
    }

    private void OnManagedControllerActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        if (!_matchFlowCoordinator.isActivated && !_matchFlowCoordinator.isActivated)
            return;
        
        FlowCoordinator activeFlowCoordinator = _matchFlowCoordinator.isActivated ? _matchFlowCoordinator : _matchmakingMenuFlowCoordinator;
        
        SetupManagedController(activeFlowCoordinator);
    }

    private void OnManagedControllerDeactivated(bool removedFromHierarchy, bool screenSystemDisabling) => ResetManagedController();
    
    protected virtual void SetupManagedController(FlowCoordinator parentFlowCoordinator){}
    protected virtual void ResetManagedController(){}
}