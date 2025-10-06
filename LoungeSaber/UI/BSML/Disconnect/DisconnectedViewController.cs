using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using JetBrains.Annotations;

namespace LoungeSaber.UI.BSML.Disconnect;

[ViewDefinition("LoungeSaber.UI.BSML.Disconnect.DisconnectedView.bsml")]
public class DisconnectedViewController : BSMLAutomaticViewController
{
    [CanBeNull] private Action _onOkButtonClickedCallback = null;
    
    [UIValue("reasonText")] private string ReasonText { get; set; } = "";

    public void SetReason(string reason, Action onOkButtonClickedCallback)
    {
        _onOkButtonClickedCallback = onOkButtonClickedCallback;
        
        ReasonText = $"Reason: {reason}";
        NotifyPropertyChanged(nameof(ReasonText));
    }

    [UIAction("okButtonOnClick")]
    private void OkButtonOnClick()
    {
        _onOkButtonClickedCallback?.Invoke();
        _onOkButtonClickedCallback = null;
    }
}