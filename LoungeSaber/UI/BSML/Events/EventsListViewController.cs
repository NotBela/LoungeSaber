using System.Collections;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Interfaces;
using LoungeSaber.UI.BSML.Components;
using Zenject;

namespace LoungeSaber.UI.BSML.Events;

[ViewDefinition("LoungeSaber.UI.BSML.Events.EventsListView.bsml")]
public class EventsListViewController : BSMLAutomaticViewController
{
    [Inject] private readonly ILoungeSaberApi _loungeSaberApi = null;
    
    [UIParams] private readonly BSMLParserParams _parserParams = null;
    
    [UIComponent("eventsList")] private readonly CustomCellListTableData _eventsList = null;
    
    public bool Parsed { get; private set; }
    
    [UIAction("#post-parse")]
    private void PostParse() => Parsed = true;

    public async Task RefreshData()
    {
        _parserParams.EmitEvent("loadingModalShow");
        
        var events = await _loungeSaberApi.GetEvents();

        _eventsList.Data = events.Select(i => new EventSlot(i)).ToList();
        _eventsList.TableView.ReloadData();
        
        _parserParams.EmitEvent("loadingModalHide");
    }
}