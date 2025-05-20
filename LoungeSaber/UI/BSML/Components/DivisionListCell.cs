using BeatSaberMarkupLanguage.Attributes;
using LoungeSaber.Managers;
using LoungeSaber.Models.Divisions;
using TMPro;
using UnityEngine;

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

            _divisionName = division.DivisionName;
            _divisionDescription = "test";
        }

        [UIAction("joinButtonOnClick")]
        private void JoinButtonOnClick()
        {
            
        }
    }
}