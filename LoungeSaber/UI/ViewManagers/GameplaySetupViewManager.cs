using HarmonyLib;
using Zenject;

namespace LoungeSaber.UI.ViewManagers;

public class GameplaySetupViewManager : IInitializable, IDisposable
{
    [field: Inject]
    public GameplaySetupViewController ManagedController { get; } = null;

    private void SetupView()
    {
        ManagedController.Setup(true, true,true, false, PlayerSettingsPanelController.PlayerSettingsPanelLayout.Singleplayer);
        
        ManagedController._gameplayModifiersPanelController._gameplayModifierToggles
            .Where(i => i.name != "ProMode")
            .Do(i => i.gameObject.SetActive(false));
    }

    private void ResetView()
    {
        ManagedController._gameplayModifiersPanelController._gameplayModifierToggles.Do(i => i.gameObject.SetActive(true));
    }

    public void Initialize()
    {
        ManagedController.didActivateEvent += OnActivated;
        ManagedController.didDeactivateEvent += OnDeactivated;
    }

    private void OnDeactivated(bool removedFromHierarchy, bool screenSystemDisabling)
    {
        ResetView();
    }

    private void OnActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        if (!ViewManager.Active)
            return;
        
        SetupView();
    }

    public void Dispose()
    {
        ManagedController.didActivateEvent -= OnActivated;
        ManagedController.didDeactivateEvent -= OnDeactivated;
    }
}