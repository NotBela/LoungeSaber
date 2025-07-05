using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Server;
using SiraUtil.Logging;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.BSML.Leaderboard
{
    [ViewDefinition("LoungeSaber.UI.BSML.Leaderboard.LeaderboardPanelView.bsml")]
    public class LeaderboardPanelViewController : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        [Inject] private readonly LoungeSaberLeaderboardViewController _loungeSaberLeaderboardViewController = null;
        
        private bool _isLoading = false;
        
        private bool IsLoading
        {
            get => _isLoading;
            set {
                _isLoading = value;
                NotifyPropertyChanged(null);
            }
        }

        [UIValue("showLoadingScreen")] private bool _showLoadingScreen => IsLoading;
        
        [UIValue("showRankingScreen")] private bool _showRankingScreen => !IsLoading;
        
        
        [UIValue("globalRankingText")] private string _globalRankingText { get; set; } = "placeholder";
        [UIValue("divisionText")] private string _divisionText { get; set; } = "placeholder";

        [UIValue("bgColor")] private string _bgColor { get; set; } = "#808080";
        
        private FloatingScreen _floatingScreen;

        public async Task UpdateRankingInfo()
        {
            IsLoading = true;
            
            var userId = (await BS_Utils.Gameplay.GetUserInfo.GetUserAsync()).platformUserId;
            
            var userData = await _loungeSaberApi.GetUserInfo(userId);

            _globalRankingText = $"<b>Global Ranking:</b> #{(userData == null ? "0" : userData.Rank)}";
            _divisionText = "placeholder"; //TODO: waiting on garrick
            
            IsLoading = false;
            
            NotifyPropertyChanged(null);
        }

        private async void OnLeaderboardActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            try
            {
                if (firstActivation)
                {
                    CreateFloatingLeaderboardPanel();
                    _floatingScreen.gameObject.SetActive(true);
                    await UpdateRankingInfo();
                    return;
                }
            
                _floatingScreen.gameObject.SetActive(true);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        private void OnLeaderboardDeactivated(bool removedFromHierarchy, bool screenSystemDisabling) => _floatingScreen.gameObject.SetActive(false);

        private void CreateFloatingLeaderboardPanel()
        {
            _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(100f, 25f), false, Vector2.zero, Quaternion.identity);
            _floatingScreen.SetRootViewController(this, AnimationType.In);
            _floatingScreen.name = "LoungeSaberFloatingScreen";
            
            _floatingScreen.transform.SetParent(, false);
            _floatingScreen.transform.localPosition = new Vector3(3f, 50f);
            _floatingScreen.transform.localScale = Vector3.one;
        }

        public void Initialize()
        {
            _loungeSaberLeaderboardViewController.didDeactivateEvent += OnLeaderboardDeactivated;
            _loungeSaberLeaderboardViewController.didActivateEvent += OnLeaderboardActivated;
        }

        public void Dispose()
        {
            _loungeSaberLeaderboardViewController.didActivateEvent -= OnLeaderboardActivated;
            _loungeSaberLeaderboardViewController.didDeactivateEvent -= OnLeaderboardDeactivated;
        }
    }
}