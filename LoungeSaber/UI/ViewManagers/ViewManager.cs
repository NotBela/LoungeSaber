using HMUI;
using LoungeSaber.UI.FlowCoordinators;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.ViewManagers;

public abstract class ViewManager : MonoBehaviour, IInitializable, IDisposable
{
    public abstract ViewController ManagedController { get; }
    
    public static bool Active { get; set; } = false;

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
        if (!Active) 
            return;
        
        SetupManagedController();
    }

    private void OnManagedControllerDeactivated(bool removedFromHierarchy, bool screenSystemDisabling) => ResetManagedController();
    
    protected virtual void SetupManagedController(){}
    protected virtual void ResetManagedController(){}
}