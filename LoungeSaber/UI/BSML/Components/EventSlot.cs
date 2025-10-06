using BeatSaberMarkupLanguage.Attributes;
using LoungeSaber.Models.Events;
using UnityEngine.UI;

namespace LoungeSaber.UI.BSML.Components;

public class EventSlot(EventData eventData)
{
    [UIComponent("joinButton")] private readonly Button _joinButton = null;
    
    [UIValue("displayName")] private string _displayName = eventData.DisplayName;
    [UIValue("description")] private string _description = eventData.Description;
    
    public event Action<EventData> OnJoinButtonClicked;

    [UIAction("joinButtonOnClick")]
    private void JoinButtonOnClick()
    {
        OnJoinButtonClicked?.Invoke(EventData);
    }
    
    public readonly EventData EventData = eventData;
}