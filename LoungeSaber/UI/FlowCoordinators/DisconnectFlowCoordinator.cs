using HMUI;
using LoungeSaber.Extensions;
using LoungeSaber.UI.BSML.Disconnect;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators;

public class DisconnectFlowCoordinator : FlowCoordinator
{
    [Inject] private readonly DisconnectedViewController _disconnectedViewController = null;
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        SetTitle("LoungeSaber");
        showBackButton = false;
        ProvideInitialViewControllers(_disconnectedViewController);
    }

    public void Setup(string reason, Action callback)
    {
        _disconnectedViewController.SetReason(reason, callback);
    }
}