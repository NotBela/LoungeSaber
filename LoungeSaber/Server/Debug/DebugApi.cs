using IPA.Utilities;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Server;
using LoungeSaber.Models.UserInfo;
using Zenject;

namespace LoungeSaber.Server.Debug;

public class DebugApi : ILoungeSaberApi
{
    public Task<Models.UserInfo.UserInfo> GetUserInfo(string id)
    {
        return Task.FromResult(new Models.UserInfo.UserInfo(
            "self",
            "0",
            1000,
            new DivisionInfo(DivisionInfo.DivisionName.Bronze, 1, "#000000"),
            null,
            1,
            null,
            false));
    }

    public Task<Models.UserInfo.UserInfo[]> GetLeaderboardRange(int start, int range)
    {
        return Task.FromResult(Array.Empty<Models.UserInfo.UserInfo>());
    }

    public Task<Models.UserInfo.UserInfo[]> GetAroundUser(string id)
    {
        return Task.FromResult(Array.Empty<Models.UserInfo.UserInfo>());
    }

    public Task<ServerStatus> GetServerStatus()
    {
        return Task.FromResult(new ServerStatus([UnityGame.GameVersion.ToString()], ["1.0.0"],
            ServerStatus.ServerState.Online));
    }

    public Task<string[]> GetMapHashes()
    {
        return Task.FromResult<string[]>(["28aef5dabe5581b81a2b5d7452534bfbf32b2722", "807e71eb310b8aeba98a643c3e8c390e24e89a80", "cdac5e8a2285e74e959f824feb791d85cf825f8c"]);
    }
}