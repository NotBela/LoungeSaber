using System.Numerics;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using HarmonyLib;
using HMUI;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.Components;
using SiraUtil.Logging;
using TMPro;
using Zenject;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

namespace LoungeSaber.UI.BSML.Leaderboard
{
    [ViewDefinition("LoungeSaber.UI.BSML.Leaderboard.LoungeSaberLeaderboardView.bsml")]
    public class LoungeSaberLeaderboardViewController : BSMLAutomaticViewController
    { 
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        [UIComponent("leaderboardList")] private readonly CustomCellListTableData _leaderboardList = null;

        [UIAction("#post-parse")]
        async void PostParse()
        {
            try
            {
                var leaderboardUsers = await _loungeSaberApi.GetLeaderboardRange(0, 10);
                
                _leaderboardList.Data = leaderboardUsers.Select(i => new LeaderboardSlot(i)).ToList();
                _leaderboardList.TableView.ReloadData();
                
                leaderboardUsers.Do(i => _siraLog.Info(i.Username));
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }
}