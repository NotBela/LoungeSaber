using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using LoungeSaber.UI.BSML.Events;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators.Events;

public class EventsFlowCoordinator : FlowCoordinator
{
    [Inject] private readonly EventsListViewController _eventsListViewController = null;
    [Inject] private readonly SiraLog _siraLog = null;
    
    public event Action OnBackButtonPressed; 
    
    protected override async void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        try
        {
            SetTitle("Events");
            showBackButton = true;
            ProvideInitialViewControllers(_eventsListViewController);

            while (!_eventsListViewController.Parsed)
                await Task.Delay(25);

            await _eventsListViewController.RefreshData();
        }
        catch (Exception e)
        {
            _siraLog.Error(e);
        }
    }

    protected override void BackButtonWasPressed(ViewController _) => OnBackButtonPressed?.Invoke();
}