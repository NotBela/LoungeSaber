using System;
using System.Linq;
using System.Threading.Tasks;
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
        
        
        
        private void SetDivisionListData(Division[] data)
        {
            try
            {
                _divisionList.Data = data.Select(i => new DivisionListCell(i, _stateManager, _siraLog)).ToArray();
                _divisionList.TableView.ReloadData();
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        public void Initialize()
        {
            _stateManager.OnDivisionDataRefreshed += OnDivisionListRefreshed;
        }

        private void OnDivisionListRefreshed()
        {
            SetDivisionListData(_loungeSaberApi.Divisions);
        }

        public void Dispose()
        {
            _stateManager.OnDivisionDataRefreshed -= OnDivisionListRefreshed;
        }
    }
}