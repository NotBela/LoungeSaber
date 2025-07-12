using HarmonyLib;
using HMUI;
using IPA.Utilities;
using LoungeSaber.Configuration;
using LoungeSaber.Game;
using LoungeSaber.Models.Server;
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
    
    [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
    [Inject] private readonly MapDownloader _mapDownloader = null;
    
    [Inject] private readonly SiraLog _siraLog = null;
    
    [Inject] private readonly PluginConfig _config = null;

    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        showBackButton = false;
        SetTitle("LoungeSaber");

        ProvideInitialViewControllers(_checkingServerStatusViewController);
        _checkingServerStatusViewController.SetControllerState(CheckingServerStatusViewController.ControllerState
            .CheckingServer);

        Task.Run(async Task () =>
        {
            await StartServerCheckingExecutionFlow();
        });
    }

    private async Task StartServerCheckingExecutionFlow()
    {
        var serverResponse = await _loungeSaberApi.GetServerStatus();

        if (serverResponse == null)
        {
            ReplaceViewControllerSynchronously(_cantConnectToServerViewController);
            _cantConnectToServerViewController.SetReasonText("InvalidServerResponse");
            return;
        }

        if (!serverResponse.AllowedModVersions.Contains(IPA.Loader.PluginManager.GetPluginFromId("LoungeSaber").HVersion
                .ToString()))
        {
            ReplaceViewControllerSynchronously(_cantConnectToServerViewController);
            _cantConnectToServerViewController.SetReasonText("OutdatedPluginVersion");
            return;
        }

        if (!serverResponse.AllowedGameVersions.Contains(UnityGame.GameVersion.ToString()))
        {
            ReplaceViewControllerSynchronously(_cantConnectToServerViewController);
            _cantConnectToServerViewController.SetReasonText("OutdatedGameVersion");
            return;
        }

        if (serverResponse.State != ServerStatus.ServerState.Online)
        {
            ReplaceViewControllerSynchronously(_cantConnectToServerViewController);
            _cantConnectToServerViewController.SetReasonText("ServerInMaintenance");
            return;
        }
        
        _checkingServerStatusViewController.SetControllerState(CheckingServerStatusViewController.ControllerState
            .CheckingMaps);

        var maps = await _loungeSaberApi.GetMapHashes();
        var missingMapHashes = maps.Where(i => Loader.GetLevelByHash(i) == null).ToArray();
        
        if (missingMapHashes.Length == 0)
        {
            PresentFlowCoordinatorSynchronously(_matchmakingMenuFlowCoordinator);
            return;
        }

        if (_config.DownloadMapsAutomatically)
        {
            await DownloadMaps(missingMapHashes);
            return;
        }

        ReplaceViewControllerSynchronously(_missingMapsViewController);
        _missingMapsViewController.SetMissingMapCount(missingMapHashes.Length);

        _missingMapsViewController.UserChoseToDownloadMaps += OnChoiceRecieved;
        return;
        
        void OnChoiceRecieved(bool choice)
        {
            _missingMapsViewController.UserChoseToDownloadMaps -= OnChoiceRecieved;
            
            if (choice)
            {
                Task.Run(async Task () =>
                {
                    await DownloadMaps(missingMapHashes);
                });
                
                return;
            }
            
            _mainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
    
    private async Task DownloadMaps(string[] missingMapHashes)
    {
        try
        {
            await Task.Delay(1000);
            ReplaceViewControllerSynchronously(_checkingServerStatusViewController);
            _checkingServerStatusViewController.SetControllerState(CheckingServerStatusViewController.ControllerState
                .CheckingMaps);

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
            _cantConnectToServerViewController.SetReasonText("MapDownloadException");
        }
    }
}