using BeatSaberMarkupLanguage.Attributes;
using LoungeSaber.Managers;
using LoungeSaber.Models.Divisions;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

namespace LoungeSaber.UI.BSML.Components
{
    public class DivisionListCell
    {
        private readonly StateManager _stateManager;
        
        public Division Division { get; private set; }
        
        [UIValue("divisionName")] private string _divisionName;
        [UIValue("divisionDescription")] private string _divisionDescription;

        public DivisionListCell(Division division, StateManager stateManager)
        {
            _stateManager = stateManager;
            
            Division = division;
            
            Plugin.Log.Info($"{division.DivisionColor.r} {division.DivisionColor.g} {division.DivisionColor.b}");

            _divisionName = $"<color={division.DivisionColor.ToHexidecimal()}><b>{division.DivisionName} Division";
            _divisionDescription = "test";
        }

        [UIAction("joinButtonOnClick")]
        private void JoinButtonOnClick()
        {
            _stateManager.JoinRoom(Division);
        }
    }
}