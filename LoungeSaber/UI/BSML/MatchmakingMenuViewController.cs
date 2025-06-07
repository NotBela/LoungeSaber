using System;
using System.Diagnostics;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Server;
using SiraUtil.Logging;
using UnityEngine.UI;
using Zenject;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.MatchmakingMenuView.bsml")]
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
                _joinMatchmakingPoolButton.interactable = false;
                _joinMatchmakingPoolButton.SetButtonText("Finding Match (Joining Pool...)");
                // await _serverListener.Connect();
                
                _matchmakingTimeStopwatch.Restart();
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        public void Tick()
        {
            if (!_matchmakingTimeStopwatch.IsRunning) return;
            
            _joinMatchmakingPoolButton.SetButtonText($"Finding Match ({(_matchmakingTimeStopwatch.Elapsed.Minutes):00}:{(_matchmakingTimeStopwatch.Elapsed.Seconds % 60):00} elapsed)");
        }
    }
}