using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using JetBrains.Annotations;

namespace LoungeSaber.UI.BSML.Disconnect;

[ViewDefinition("LoungeSaber.UI.BSML.Disconnect.DisconnectedView.bsml")]
public class DisconnectedViewController : BSMLAutomaticViewController
{
    [CanBeNull] public Action OnOkButtonClicked;
    
    [UIValue("reasonText")] private string ReasonText { get; set; } = "";

    public void SetReason(string reason)
    {
        ReasonText = $"Reason: {reason}";
        NotifyPropertyChanged(nameof(ReasonText));
    }

    [UIAction("okButtonOnClick")]
    private void OkButtonOnClick() => OnOkButtonClicked?.Invoke();
}