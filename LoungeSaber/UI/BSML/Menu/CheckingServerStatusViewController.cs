using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Game;
using Zenject;

namespace LoungeSaber.UI.BSML.Menu;

[ViewDefinition("LoungeSaber.UI.BSML.Menu.CheckingServerStatusView.bsml")]
public class CheckingServerStatusViewController : BSMLAutomaticViewController
{
    [Inject] private readonly MapDownloader _mapDownloader = null;
    
    [UIValue("stateText")] private string _stateText { get; set; } = "placeholder";
    
    public enum ControllerState
    {
        CheckingServer, 
        CheckingMaps,
        DownloadingMaps
    }

    public void SetControllerState(ControllerState state)
    {
        switch (state)
        {
            case ControllerState.CheckingServer:
                _stateText = "Connecting to server...";
                break;
            case ControllerState.CheckingMaps:
                _stateText = "Fetching maps...";
                break;
            case ControllerState.DownloadingMaps:
                _stateText = "Downloading maps...";
                _mapDownloader.OnMapDownloaded += OnMapDownloaded;
                break;
        };
        NotifyPropertyChanged(nameof(_stateText));
    }

    private void OnMapDownloaded(int mapsDownloaded, int totalMaps)
    {
        if (mapsDownloaded == totalMaps)
            _mapDownloader.OnMapDownloaded -= OnMapDownloaded;
        
        _stateText = $"Downloading maps... ({mapsDownloaded}/{totalMaps})";
        
        NotifyPropertyChanged(nameof(_stateText));
    }
}