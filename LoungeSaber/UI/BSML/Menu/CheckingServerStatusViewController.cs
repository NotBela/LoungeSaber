using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using Zenject;

namespace LoungeSaber.UI.BSML.Menu;

[ViewDefinition("LoungeSaber.UI.BSML.Menu.CheckingServerStatusView.bsml")]
public class CheckingServerStatusViewController : BSMLAutomaticViewController
{
    [UIValue("stateText")] private string _stateText { get; set; } = "placeholder";
    
    public enum ControllerState
    {
        CheckingServer, 
        CheckingMaps,
        DownloadingMaps
    }
    
    private ControllerState _state = ControllerState.CheckingServer;

    public void SetControllerState(ControllerState state)
    {
        _state = state;

        _stateText = _state switch
        {
            ControllerState.CheckingServer => "Connecting to server...",
            ControllerState.CheckingMaps => "Fetching maps...",
            ControllerState.DownloadingMaps => "Downloading maps...",
            _ => ""
        };
        NotifyPropertyChanged(nameof(_stateText));
    }
}