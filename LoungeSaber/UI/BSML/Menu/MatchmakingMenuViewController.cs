using System;
using System.Diagnostics;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using SiraUtil.Logging;
using UnityEngine.UI;
using Zenject;

namespace LoungeSaber.UI.BSML.Menu
{
    [ViewDefinition("LoungeSaber.UI.BSML.Menu.MatchmakingMenuView.bsml")]
    public class MatchmakingMenuViewController : BSMLAutomaticViewController, ITickable
    {
        [Inject] private readonly ServerListener _serverListener = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        private readonly Stopwatch _matchmakingTimeStopwatch = new Stopwatch();
        
        [UIComponent("JoinMatchmakingPoolButton")] private readonly Button _joinMatchmakingPoolButton = null;

        [UIAction("JoinMatchmakingPoolButtonOnClick")]
        private async void OnJoinMatchmakingPoolButtonOnClick()
        {
            try
            {
                _matchmakingTimeStopwatch.Stop();
                _joinMatchmakingPoolButton.interactable = false;
                _joinMatchmakingPoolButton.SetButtonText("Finding Match (Joining Pool...)");

                await _serverListener.Connect(OnConnectedCallback);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        private void OnConnectedCallback(JoinResponse joinResponse)
        {
            if (joinResponse.Successful)
            {
                _matchmakingTimeStopwatch.Restart();
                return;
            }
            
            _siraLog.Warn($"Failed to connect to matchmaking pool: {joinResponse.Message}");
        }

        public void Tick()
        {
            if (isActiveAndEnabled)
                UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            if (_matchmakingTimeStopwatch.IsRunning)
                _joinMatchmakingPoolButton.SetButtonText($"Finding Match ({_matchmakingTimeStopwatch.Elapsed.Minutes:00}:{_matchmakingTimeStopwatch.Elapsed.Seconds % 60:00} elapsed)");
        }
    }
}