using System;
using BeatSaberMarkupLanguage.Attributes;
using LoungeSaber.Managers;
using LoungeSaber.Models.Divisions;
using SiraUtil.Logging;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

namespace LoungeSaber.UI.BSML.Components
{
    public class DivisionListCell
    {
        private readonly StateManager _stateManager;
        private readonly SiraLog _siraLog;
        
        public Division Division { get; private set; }
        
        [UIValue("divisionName")] private string _divisionName;
        [UIValue("divisionDescription")] private string _divisionDescription;

        public DivisionListCell(Division division, StateManager stateManager, SiraLog siraLog)
        {
            _stateManager = stateManager;
            _siraLog = siraLog;
            
            Division = division;

            _divisionName = $"<color={division.DivisionColor.ToHexidecimal()}><b>{division.DivisionName} Division";
            _divisionDescription = "test";
        }

        [UIAction("joinButtonOnClick")]
        private async void JoinButtonOnClick()
        {
            try
            {
                await _stateManager.JoinRoom(Division);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }
}