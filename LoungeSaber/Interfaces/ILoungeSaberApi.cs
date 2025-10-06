using JetBrains.Annotations;
using LoungeSaber.Models.Events;
using LoungeSaber.Models.Server;

namespace LoungeSaber.Interfaces;

public interface ILoungeSaberApi
{
    [ItemCanBeNull] 
    public Task<Models.UserInfo.UserInfo> GetUserInfo(string id);

    [ItemCanBeNull] 
    public Task<Models.UserInfo.UserInfo[]> GetLeaderboardRange(int start, int range);

    [ItemCanBeNull]
    public Task<Models.UserInfo.UserInfo[]> GetAroundUser(string id);

    [ItemCanBeNull]
    public Task<ServerStatus> GetServerStatus();

    public Task<string[]> GetMapHashes();
    
    public Task<EventData[]> GetEvents();
}