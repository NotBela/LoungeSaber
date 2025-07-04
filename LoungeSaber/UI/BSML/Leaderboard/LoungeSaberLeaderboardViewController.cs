using System.Numerics;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using SiraUtil.Logging;
using Zenject;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

namespace LoungeSaber.UI.BSML.Leaderboard
{
    [ViewDefinition("LoungeSaber.UI.BSML.Leaderboard.LoungeSaberLeaderboardView.bsml")]
    public class LoungeSaberLeaderboardViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly LeaderboardPanelViewController _leaderboardPanelViewController = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        private FloatingScreen _floatingScreen;
        
        protected override async void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            try
            {
                if (firstActivation)
                {
                    CreateFloatingLeaderboardPanel();
                    _floatingScreen.gameObject.SetActive(true);
                    await _leaderboardPanelViewController.UpdateRankingInfo();
                    return;
                }
            
                _floatingScreen.gameObject.SetActive(true);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
        
        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemEnabling) => _floatingScreen.gameObject.SetActive(false);
        
        private void CreateFloatingLeaderboardPanel()
        {
            _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(100f, 25f), true, Vector2.zero, Quaternion.identity);
            _floatingScreen.SetRootViewController(_leaderboardPanelViewController, AnimationType.In);
            _floatingScreen.name = "LoungeSaberFloatingScreen";
        }
    }
}