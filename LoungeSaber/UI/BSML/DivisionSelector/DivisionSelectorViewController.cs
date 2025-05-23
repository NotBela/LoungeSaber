using System;
using System.Linq;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Models.Divisions;
using LoungeSaber.Server.Api;
using LoungeSaber.UI.BSML.Components;
using SiraUtil.Logging;
using TMPro;
using Zenject;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.DivisionSelector.DivisonSelectorView.bsml")]
    public class DivisionSelectorViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        [Inject] private readonly SiraLog _siraLog = null;
        
        [UIComponent("divisionList")] private readonly CustomCellListTableData _divisionList = null;
        
        private void SetDivisionListData(Division[] data)
        {
            try
            {
                _divisionList.Data = data.Select(i => new DivisionListCell(i, _siraLog)).ToArray();
                _divisionList.TableView.ReloadData();
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        [UIAction("#post-parse")]
        async void PostParse()
        {
            try
            {
                var divisions = await _loungeSaberApi.FetchDivisions();
                if (divisions == null) 
                    return;
            
                _divisionList.Data = divisions.Select(i => new DivisionListCell(i, _siraLog)).ToArray();
                _divisionList.TableView.ReloadData();
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }
}