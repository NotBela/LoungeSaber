using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using JetBrains.Annotations;

namespace LoungeSaber.UI.BSML.Menu;

[ViewDefinition("LoungeSaber.UI.BSML.Menu.MissingMapsView.bsml")]
public class MissingMapsViewController : BSMLAutomaticViewController
{
    [CanBeNull] public event Action<bool> UserChoseToDownloadMaps;
    
    [UIValue("missingMapText")] private string _missingMapText { get; set; } = "placeholder";

    public void SetMissingMapCount(int missingMapCount)
    {
        _missingMapText = $"It looks like you are missing {missingMapCount} map(s).";
        NotifyPropertyChanged(nameof(_missingMapText));
    }
    
    [UIAction("yesButtonOnClick")] private void YesButtonOnClick() => UserChoseToDownloadMaps?.Invoke(true);
    [UIAction("noButtonOnClick")] private void NoButtonOnClick() => UserChoseToDownloadMaps?.Invoke(false);
}