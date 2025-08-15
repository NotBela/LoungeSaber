using System.Globalization;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaverSharp.Models;
using HarmonyLib;
using LoungeSaber.Models.Map;
using LoungeSaber.UI.BSML.Components;
using LoungeSaber.UI.BSML.Components.CustomLevelBar;
using SiraUtil.Logging;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.WaitingForMatchToStartView.bsml")]
    public class WaitingForMatchToStartViewController : BSMLAutomaticViewController, ITickable
    {
        [UIValue("matchStartTimer")] private string MatchStartTimer { get; set; } = "";

        private CustomLevelBar _customLevelBar = null;
         
        private DateTime _startTime;

        [UIAction("#post-parse")]
        private void PostParse()
        {
            _customLevelBar ??= Resources.FindObjectsOfTypeAll<CustomLevelBar>()
                .First(i => i.name == "WaitingForMatchStartLevelBar");
        }
        
        public async Task PopulateData(VotingMap votingMap, DateTime startTime)
        {
            while (_customLevelBar is null)
                await Task.Delay(25);
            
            _startTime = startTime;
            
            _customLevelBar?.Setup(votingMap);
        }

        public void Tick()
        {
            if (!isActivated)
                return;
            
            MatchStartTimer = $"Starting in {((int) (_startTime - DateTime.UtcNow).TotalSeconds).ToString(CultureInfo.InvariantCulture)}...";
            
            NotifyPropertyChanged(nameof(MatchStartTimer));
        }
    }
}