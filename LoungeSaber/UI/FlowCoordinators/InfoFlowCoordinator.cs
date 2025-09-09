using HMUI;
using LoungeSaber.Extensions;
using LoungeSaber.UI.BSML.Info;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators;

public class InfoFlowCoordinator : FlowCoordinator
{
    [Inject] private readonly InfoViewController _infoViewController = null;
    
    public event Action OnBackButtonPressed;
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        SetTitle("LoungeSaber");
        ProvideInitialViewControllers(_infoViewController);
        showBackButton = true;
    }

    protected override void BackButtonWasPressed(ViewController _) => OnBackButtonPressed?.Invoke();
}