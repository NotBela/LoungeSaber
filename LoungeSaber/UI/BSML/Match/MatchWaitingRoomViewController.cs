using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Server.Api;
using LoungeSaber.Server.MatchRoom;
using TMPro;
using Zenject;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.MatchWaitingRoomView.bsml")]
    public class MatchWaitingRoomViewController : BSMLAutomaticViewController, IDisposable
    {
        [Inject] private readonly LoungeServerInterfacer _loungeServerInterfacer = null;
        
        [UIComponent("playerVolumeText")] private readonly TextMeshProUGUI _playerVolumeText = null;

        [UIAction("#post-parse")]
        void PostParse()
        {
            _loungeServerInterfacer.UserCountUpdated += OnUserCountUpdated;
        }

        private void OnUserCountUpdated(int count)
        {
            _playerVolumeText.text = $"{count} player(s) in room";
        }

        public void Dispose()
        {
            _loungeServerInterfacer.UserCountUpdated -= OnUserCountUpdated;
        }
    }
}