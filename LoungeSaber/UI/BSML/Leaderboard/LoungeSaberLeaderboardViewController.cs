using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BGLib.Polyglot;
using HMUI;
using IPA.Utilities;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.Components;
using SiraUtil.Logging;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.BSML.Leaderboard
{
    [ViewDefinition("LoungeSaber.UI.BSML.Leaderboard.LoungeSaberLeaderboardView.bsml")]
    public class LoungeSaberLeaderboardViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly PlatformLeaderboardViewController _platformLeaderboardViewController = null;
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        [Inject] private readonly PlayerDataModel _playerDataModel = null;
        
        private bool _isLoaded = false;

        [UIValue("is-loaded")]
        private bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                _isLoaded = value;
                NotifyPropertyChanged(null);
            }
        }
        
        [UIValue("is-loading")]
        private bool IsLoading => !IsLoaded;

        [UIComponent("leaderboard")] private readonly CustomCellListTableData _leaderboard = null;

        [UIValue("cell-data")] private readonly List<IconSegmentedControl.DataItem> _cellData = new(){};

        private void Awake()
        {
            _cellData.Add(new IconSegmentedControl.DataItem(_platformLeaderboardViewController.GetField<Sprite, PlatformLeaderboardViewController>("_globalLeaderboardIcon"), Localization.Get("BUTTON_HIGHSCORES_GLOBAL")));
            _cellData.Add(new IconSegmentedControl.DataItem(_platformLeaderboardViewController.GetField<Sprite, PlatformLeaderboardViewController>("_aroundPlayerLeaderboardIcon"), Localization.Get("BUTTON_HIGHSCORES_AROUND_YOU")));
            _cellData.Add(new IconSegmentedControl.DataItem(_platformLeaderboardViewController.GetField<Sprite, PlatformLeaderboardViewController>("_friendsLeaderboardIcon"), Localization.Get("BUTTON_HIGHSCORES_FRIENDS")));
            IsLoaded = false;
        }

        protected override async void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            try
            {
                base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            
                if (!firstActivation) 
                    return;

                var topOfLeaderboard = await _loungeSaberApi.GetLeaderboardRange(0, 10);
                SetLeaderboardData(topOfLeaderboard);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        private void SetLeaderboardData(Models.UserInfo.UserInfo[] userInfo)
        {
            _leaderboard.Data = userInfo.Select(i => new LeaderboardSlot(i)).ToList();
            _leaderboard.TableView.ReloadData();
            IsLoaded = true;
        }

        private bool _upEnabled = false;

        [UIValue("up-enabled")]
        private bool UpEnabled
        {
            get => _upEnabled;
            set
            {
                _upEnabled = value;
                NotifyPropertyChanged(null);
            }
        }
        
        private bool _downEnabled = true;
        
        [UIValue("down-enabled")]
        private bool DownEnabled
        {
            get => _downEnabled;
            set
            {
                _downEnabled = value;
                NotifyPropertyChanged(null);
            }
        }
        
        private int pageNumber = 0;

        public enum LeaderboardStates
        {
            Global,
            Self,
            Friends
        }

        private LeaderboardStates CurrentState { get; set; } = LeaderboardStates.Global;

        [UIAction("cell-selected")]
        private void OnCellSelected(SegmentedControl segmentedControl, int index)
        {
            switch (index)
            {
                case 0:
                    SetLeaderboardState(LeaderboardStates.Global);
                    break;
                case 1:
                    SetLeaderboardState(LeaderboardStates.Self);
                    break;
                case 2:
                    SetLeaderboardState(LeaderboardStates.Friends);
                    break;
            }
        }

        private async void SetLeaderboardState(LeaderboardStates state)
        {
            try
            {
                if (state == CurrentState) 
                    return;
            
                CurrentState = state;

                pageNumber = 0;
            
                IsLoaded = false;
            
                switch (state)
                {
                    case LeaderboardStates.Global:
                        UpEnabled = false;
                        DownEnabled = true;
                        var topOfLeaderboard = await _loungeSaberApi.GetLeaderboardRange(0, 10);
                        SetLeaderboardData(topOfLeaderboard);
                        break;
                    case LeaderboardStates.Self:
                        UpEnabled = false;
                        DownEnabled = false;
                        var ownId = (await BS_Utils.Gameplay.GetUserInfo.GetUserAsync()).platformUserId;
                        var aroundUser = await _loungeSaberApi.GetAroundUser(ownId);
                        SetLeaderboardData(aroundUser);
                        break;
                }
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }
}