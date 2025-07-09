﻿using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using JetBrains.Annotations;

namespace LoungeSaber.UI.BSML.Menu;

[ViewDefinition("LoungeSaber.UI.BSML.Menu.CantConnectToServerView.bsml")]
public class CantConnectToServerViewController : BSMLAutomaticViewController
{
    [CanBeNull] public event Action OnContinueButtonPressed;
    
    [UIValue("reasonText")] private string _reasonText { get; set; } = "placeholder";
    
    public void SetReasonText(string reason)
    {
        _reasonText = $"Reason: {reason}";
        NotifyPropertyChanged(nameof(_reasonText));
    }
    
    [UIAction("ContinueButtonOnClick")]
    private void ContinueButtonOnClick() => OnContinueButtonPressed?.Invoke();
}