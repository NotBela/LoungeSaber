using System;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Managers;
using LoungeSaber.Models.Divisions;
using LoungeSaber.Server.Api;
using LoungeSaber.UI.BSML.Components;
using SiraUtil.Logging;
using TMPro;
using Zenject;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.DivisonSelectorView.bsml")]
    public class DivisionSelectorViewController : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        [Inject] private readonly StateManager _stateManager = null;
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        [UIComponent("divisionList")] 
        private readonly CustomCellListTableData _divisionList = null;
        
        public void Initialize()
        {
            _stateManager.StateChanged += OnStateChanged;
        }

        private async void OnStateChanged(StateManager.State state)
        {
            try
            {
                if (state != StateManager.State.DivisionSelector) 
                    return;

                await _loungeSaberApi.FetchDivisions();
                SetDivisionListData(_loungeSaberApi.Divisions);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        private void SetDivisionListData(Division[] data)
        {
            _divisionList.Data = data.Select(i => new DivisionListCell(i, _stateManager)).ToArray();
            _divisionList.TableView.ReloadData();
        }

        public void Dispose()
        {
            _stateManager.StateChanged -= OnStateChanged;
        }
    }
}