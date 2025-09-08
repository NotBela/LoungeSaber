using LoungeSaber.UI.BSML.Disconnect;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators;

public class DisconnectFlowCoordinator : SynchronousFlowCoordinator
{
    [Inject] private readonly DisconnectedViewController _disconnectedViewController = null;
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        SetTitle("LoungeSaber");
        showBackButton = false;
        ProvideInitialViewControllers(_disconnectedViewController);
    }
}