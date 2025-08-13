using LoungeSaber.Configuration;
using LoungeSaber.Game;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.Menu;
using SiraUtil.Logging;
using SongCore;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators;

public class ServerCheckingFlowCoordinator : SynchronousFlowCoordinator
{
    [Inject] private readonly MatchmakingMenuFlowCoordinator _matchmakingMenuFlowCoordinator = null;
    
    [Inject] private readonly CheckingServerStatusViewController _checkingServerStatusViewController = null;
    [Inject] private readonly CantConnectToServerViewController _cantConnectToServerViewController = null;
    [Inject] private readonly MissingMapsViewController _missingMapsViewController = null;
    
    [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;

    [Inject] private readonly InitialServerChecker _serverChecker = null;
    [Inject] private readonly MapDownloader _mapDownloader = null;
    
    [Inject] private readonly SiraLog _siraLog = null;
    [Inject] private readonly PluginConfig _config = null;

    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        showBackButton = false;
        SetTitle("LoungeSaber");

        ProvideInitialViewControllers(_checkingServerStatusViewController);
        _checkingServerStatusViewController.SetControllerState(InitialServerChecker.ServerCheckingStates.ServerStatus);

        _cantConnectToServerViewController.OnContinueButtonPressed += OnContinueButtonPressed;

        _serverChecker.ServerCheckFailed += OnServerCheckFailed;
        _serverChecker.ServerCheckFinished += ServerCheckFinished;
        _serverChecker.StartMapDownload += OnStartMapDownload;

        Task.Run(async Task () => { await _serverChecker.CheckServer(); });
    }

    private void OnStartMapDownload(string[] missingMapHashes)
    {
        if (_config.DownloadMapsAutomatically)
        {
            UserChoseToDownloadMaps(true);
            return;
        }
        
        ReplaceViewControllerSynchronously(_missingMapsViewController);
        _missingMapsViewController.SetMissingMapCount(missingMapHashes.Length);
        
        _missingMapsViewController.UserChoseToDownloadMaps += UserChoseToDownloadMaps;
        return;

        async void UserChoseToDownloadMaps(bool choice)
        {
            try
            {
                if (choice)
                {
                    await DownloadMaps(missingMapHashes);
                    return;
                }
                
                _mainFlowCoordinator.DismissFlowCoordinator(this);
            }
            catch(Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }

    private void ServerCheckFinished() => PresentFlowCoordinatorSynchronously(_matchmakingMenuFlowCoordinator);

    private void OnServerCheckFailed(string reason)
    {
        _serverChecker.ServerCheckFinished -= ServerCheckFinished;
        
        ReplaceViewControllerSynchronously(_cantConnectToServerViewController);
        _cantConnectToServerViewController.SetReasonText(reason);
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
        _cantConnectToServerViewController.OnContinueButtonPressed -= OnContinueButtonPressed;
        
        _serverChecker.ServerCheckFailed -= OnServerCheckFailed;
        _serverChecker.ServerCheckFinished -= ServerCheckFinished;
        _serverChecker.StartMapDownload -= OnStartMapDownload;
    }

    private void OnContinueButtonPressed() => _mainFlowCoordinator.DismissFlowCoordinator(this);
    
    private async Task DownloadMaps(string[] missingMapHashes)
    {
        try
        {
            ReplaceViewControllerSynchronously(_checkingServerStatusViewController);
            _checkingServerStatusViewController.SetControllerState(InitialServerChecker.ServerCheckingStates.DownloadingMaps);

            await _mapDownloader.DownloadMaps(missingMapHashes);
            
            showBackButton = true;
            Loader.Instance.RefreshSongs();

            while (Loader.AreSongsLoading)
                await Task.Delay(25);
            
            PresentFlowCoordinatorSynchronously(_matchmakingMenuFlowCoordinator);
        }
        catch (Exception ex)
        {
            _siraLog.Error(ex);
            showBackButton = true;
            ReplaceViewControllerSynchronously(_cantConnectToServerViewController);
            _cantConnectToServerViewController.SetReasonText("Unhandled exception downloading beatmaps, please check your logs for more details!");
        }
    }
}