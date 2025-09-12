#nullable enable
using IPA.Utilities;
using LoungeSaber.Configuration;
using LoungeSaber.Game;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Server;
using LoungeSaber.UI.BSML.Menu;
using SongCore;
using Zenject;

namespace LoungeSaber.Server;

public class InitialServerChecker
{
    [Inject] private readonly ILoungeSaberApi _loungeSaberApi = null!;
    [Inject] private readonly IPlatformUserModel _platformUserModel = null!;
    
    public event Action<string>? ServerCheckFailed;

    public event Action<ServerCheckingStates>? ServerCheckingStateUpdated;

    // keeping this around bc i will need it for the leaderboard redesign
    public event Action<Models.UserInfo.UserInfo?>? OnUserInfoFetched;

    public event Action? ServerCheckFinished;

    public event Action<string[]>? StartMapDownload;
    
    public async Task CheckServer()
    {
        if (!await CheckServerState())
            return;
        if (!await CheckUserData())
            return;
        if (!await CheckMaps())
            return;
        
        ServerCheckFinished?.Invoke();
    }

    private async Task<bool> CheckMaps()
    {
        ServerCheckingStateUpdated?.Invoke(ServerCheckingStates.Maps);
        
        while (Loader.AreSongsLoading)
            await Task.Delay(25);

        var maps = await _loungeSaberApi.GetMapHashes();
        var missingMapHashes = maps.Where(i => Loader.GetLevelByHash(i) == null).ToArray();
        
        if (missingMapHashes.Length == 0)
        {
            return true;
        }
        
        StartMapDownload?.Invoke(missingMapHashes);
        return false;
    }

    private async Task<bool> CheckUserData()
    {
        ServerCheckingStateUpdated?.Invoke(ServerCheckingStates.UserData);

        var userData = await _loungeSaberApi.GetUserInfo((await _platformUserModel.GetUserInfo(CancellationToken.None)).platformUserId);
        
        if (userData != null && userData?.Banned == true)
        {
            ServerCheckFailed?.Invoke("You have been banned from LoungeSaber.");
            return false;
        }
        
        OnUserInfoFetched?.Invoke(userData);
        return true;
    }
    
    private async Task<bool> CheckServerState()
    {
        var serverResponse = await _loungeSaberApi.GetServerStatus();

        if (serverResponse == null)
        {
            ServerCheckFailed?.Invoke("InvalidServerResponse");
            return false;
        }

        if (!serverResponse.AllowedModVersions.Contains(IPA.Loader.PluginManager.GetPluginFromId("LoungeSaber").HVersion
                .ToString()))
        {
            ServerCheckFailed?.Invoke("OutdatedPluginVersion");
            return false;
        }

        if (!serverResponse.AllowedGameVersions.Contains(UnityGame.GameVersion.ToString()))
        {
            ServerCheckFailed?.Invoke("OutdatedGameVersion");
            return false;
        }

        if (serverResponse.State == ServerStatus.ServerState.Online) 
            return true;
        
        ServerCheckFailed?.Invoke("ServerInMaintenance");
        return false;

    }

    public enum ServerCheckingStates
    {
        ServerStatus,
        UserData,
        Maps,
        DownloadingMaps
    }
}