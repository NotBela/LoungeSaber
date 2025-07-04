using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Server;
using Zenject;

namespace LoungeSaber.UI.BSML.Leaderboard
{
    [ViewDefinition("LoungeSaber.UI.BSML.Leaderboard.LeaderboardPanelView.bsml")]
    public class LeaderboardPanelViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        
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
        
        
        [UIValue("globalRankingText")] private string _globalRankingText = "placeholder";
        [UIValue("divisionText")] private string _divisionText = "placeholder";

        [UIValue("bgColor")] private string _bgColor = "#FF0000";
        
        public async Task UpdateRankingInfo()
        {
            IsLoading = true;
            
            var userId = (await BS_Utils.Gameplay.GetUserInfo.GetUserAsync()).platformUserId;
            
            var userData = await _loungeSaberApi.GetUserInfo(userId);

            _globalRankingText = $"Global Ranking: #{(userData == null ? "0" : userData.Rank)}";
            _divisionText = "placeholder"; //TODO: waiting on garrick
            
            IsLoading = false;
            
            NotifyPropertyChanged(null);
        }
    }
}