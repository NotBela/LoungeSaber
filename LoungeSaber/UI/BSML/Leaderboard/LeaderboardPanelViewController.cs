using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
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
        [Inject] private readonly IPlatformUserModel _platformUserModel = null;
        
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
        
        [UIValue("showDivisionScreen")] private bool _showRankingScreen => !IsLoading;
        
        
        [UIValue("divisionText")] private string _globalRankingText { get; set; } = "placeholder";

        [UIValue("bgColor")] private string _bgColor { get; set; } = "#808080";
        
        private FloatingScreen _floatingScreen;

        public async Task UpdateRankingInfo()
        {
            IsLoading = true;
            
            var userData = await _loungeSaberApi.GetUserInfo((await _platformUserModel.GetUserInfo(CancellationToken.None)).platformUserId);

            _globalRankingText = "<b>Division: </b>" + (userData == null ? "None" : $"{userData.Division.Division} {userData.Division.SubDivision}");
            _bgColor = userData?.Division.Color ?? "#808080";
            
            IsLoading = false;
            
            NotifyPropertyChanged(null);
        }

        private async void OnLeaderboardActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            try
            {
                if (firstActivation)
                {
                    var screenSystem = Resources.FindObjectsOfTypeAll<ScreenSystem>().First();
                    
                    CreateFloatingLeaderboardPanel();
                    _floatingScreen.ScreenPosition = screenSystem.rightScreen.transform.position;
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
            _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(100f, 25f), false, Vector3.zero, Quaternion.identity);
            _floatingScreen.SetRootViewController(this, AnimationType.In);
            _floatingScreen.name = "LoungeSaberFloatingScreen";
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