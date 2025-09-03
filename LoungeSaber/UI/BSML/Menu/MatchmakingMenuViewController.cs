using System.Collections;
using System.Diagnostics;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using LoungeSaber.UI.FlowCoordinators;
using SiraUtil.Logging;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LoungeSaber.UI.BSML.Menu
{
    [ViewDefinition("LoungeSaber.UI.BSML.Menu.MatchmakingMenuView.bsml")]
    public class MatchmakingMenuViewController : BSMLAutomaticViewController, ITickable
    {
        [Inject] private readonly ServerListener _serverListener = null;
        [Inject] private readonly SiraLog _siraLog = null;

        [UIParams] private readonly BSMLParserParams _parserParams = null;

        public event Action AboutButtonClicked;
        
        protected override void DidDeactivate(bool firstActivation, bool addedToHierarchy)
        {
            _leaveMatchmakingPoolButton.gameObject.SetActive(false);
            ResetMatchmakingTimer();
            ChangeButtonState(false);
        }

        [UIAction("aboutButtonOnClick")]
        private void AboutButtonOnClick() => AboutButtonClicked?.Invoke();

        [UIComponent("aboutButton")] private readonly Button _aboutButton = null;

        [UIValue("showDiscordLinkModal")] private bool ShowDiscordLinkModal => false;

        private void ChangeButtonState(bool inMatch)
        {
            _aboutButton.interactable = !inMatch;
            _leaveMatchmakingPoolButton.gameObject.SetActive(inMatch);
            _joinMatchmakingPoolButton.interactable = !inMatch;
            _leaveMatchmakingPoolButton.gameObject.SetActive(inMatch);
        }

        #region Queue Control
        private readonly Stopwatch _matchmakingTimeStopwatch = new();
        
        [UIComponent("joinMatchmakingPoolButton")] private readonly Button _joinMatchmakingPoolButton = null;
        [UIComponent("leaveMatchmakingPoolButton")] private readonly Button _leaveMatchmakingPoolButton = null;
        
        private void ResetMatchmakingTimer()
        {
            _matchmakingTimeStopwatch.Stop();
            _joinMatchmakingPoolButton.interactable = true;
            _joinMatchmakingPoolButton.SetButtonText("Find Match");
        }
        

        [UIAction("leaveMatchmakingPoolButtonOnClick")]
        private void LeaveMatchmakingPoolButtonOnClick() => _parserParams.EmitEvent("disconnectModalShowEvent");

        [UIAction("leaveMatchmakingPoolDenyButtonOnClick")]
        private void LeaveMatchmakingPoolDenyButtonOnClick() =>
            _parserParams.EmitEvent("disconnectModalHideEvent");

        [UIAction("leaveMatchmakingPoolAllowButtonOnClick")]
        private void LeaveMatchmakingPoolAllowButton()
        {
            ChangeButtonState(false);
            
            _parserParams.EmitEvent("disconnectModalHideEvent");
            ResetMatchmakingTimer();
            _serverListener.Disconnect();
        }
        
        [UIAction("joinMatchmakingPoolButtonOnClick")]
        private async void JoinMatchmakingPoolButtonOnClick()
        {
            try
            {
                ChangeButtonState(true);
                _matchmakingTimeStopwatch.Stop();
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
                _joinMatchmakingPoolButton.SetButtonText($"Finding Match\n({_matchmakingTimeStopwatch.Elapsed.Minutes:00}:{_matchmakingTimeStopwatch.Elapsed.Seconds % 60:00} elapsed)");
        }
        #endregion
    }
}