using IPA.Utilities;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Events;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Server;
using LoungeSaber.Models.UserInfo;
using Zenject;

namespace LoungeSaber.Server.Debug;

public class DebugApi : ILoungeSaberApi
{
    public static readonly VotingMap[] Maps =
    [
        new("44d8d1c7c5821a7f1929542cab49c906c9e585e4", VotingMap.DifficultyType.ExpertPlus, VotingMap.CategoryType.Tech), 
        new("44d8d1c7c5821a7f1929542cab49c906c9e585e4", VotingMap.DifficultyType.ExpertPlus, VotingMap.CategoryType.Balanced),
        new("44d8d1c7c5821a7f1929542cab49c906c9e585e4", VotingMap.DifficultyType.ExpertPlus, VotingMap.CategoryType.Extreme)
    ];

    public static readonly Models.UserInfo.UserInfo DebugOpponent = new("debugOpponent", "1", 1000,
        new DivisionInfo("Iron", 4, "#111111"), new Badge("test", "#111111", false), 2, null,
        false);

    public static readonly Models.UserInfo.UserInfo Self = new(
        "self",
        "0",
        1000,
        new DivisionInfo("Iron", 1, "#000000"),
        null,
        1,
        null,
        false);
    
    public Task<Models.UserInfo.UserInfo> GetUserInfo(string id)
    {
        return Task.FromResult(Self);
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

    public async Task<string[]> GetMapHashes()
    {
        await Task.Delay(1000);
        return Maps.Select(i => i.Hash).ToArray();
    }

    public async Task<EventData[]> GetEvents()
    {
        await Task.Delay(500);
        return [new EventData("test", "test event", "test desc")];
    }
}