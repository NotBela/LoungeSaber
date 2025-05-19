using System;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Managers;
using LoungeSaber.Models.Divisions;
using LoungeSaber.Server.Api;
using LoungeSaber.UI.BSML.Components;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.DivisionSelectorView.bsml")]
    public class DivisionSelectorViewController : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        [Inject] private readonly StateManager _stateManager = null;
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        
        [UIComponent("divisionList")] private readonly CustomCellListTableData _divisionList = null;
        
        public void Initialize()
        {
            _stateManager.StateChanged += OnStateChanged;
        }

        private void OnStateChanged(StateManager.State state)
        {
            if (state != StateManager.State.DivisionSelector) 
                return;
            
            SetDivisionListData(_loungeSaberApi.Divisions);
        }

        private void SetDivisionListData(Division[] data)
        {
            _divisionList.Data = data.Select(i => new DivisionListCell(i, _stateManager)).ToList();
            _divisionList.TableView.ReloadData();
        }

        public void Dispose()
        {
            _stateManager.StateChanged -= OnStateChanged;
        }
    }
}