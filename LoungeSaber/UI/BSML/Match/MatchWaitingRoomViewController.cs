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

        [UIValue("playerCountText")] private string PlayerCountText { get; set; } = "0 player(s) in room";

        [UIAction("#post-parse")]
        void PostParse()
        {
            _loungeServerInterfacer.UserCountUpdated += OnUserCountUpdated;
        }

        private void OnUserCountUpdated(int count)
        {
            PlayerCountText = $"{count} player(s) in room";
        }

        public void Dispose()
        {
            _loungeServerInterfacer.UserCountUpdated -= OnUserCountUpdated;
        }
    }
}