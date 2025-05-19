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
        
        [UIValue("divisionName")] private string divisionName;
        [UIValue("divisionDescription")] private string divisionDescription;
        
        [UIComponent("divisionNameText")] private readonly TextMeshProUGUI _divisionNameText;

        public DivisionListCell(Division division, StateManager stateManager)
        {
            _stateManager = stateManager;
            
            Division = division;
            _divisionNameText.color = division.DivisionColor.ToUnity();
        }

        [UIAction("joinButtonOnClick")]
        private void JoinButtonOnClick()
        {
            
        }
    }
}