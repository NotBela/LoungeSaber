using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace LoungeSaber.UI.BSML.Events;

[ViewDefinition("LoungeSaber.UI.BSML.Events.EventWaitingOnNextMatchView.bsml")]
public class EventWaitingOnNextMatchViewController : BSMLAutomaticViewController
{
    [UIValue("centerText")] private string _centerText { get; set; }

    public void SetText(string text)
    {
        _centerText = text;
        NotifyPropertyChanged(nameof(_centerText));
    }
}