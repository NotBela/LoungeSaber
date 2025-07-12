using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using JetBrains.Annotations;
using LoungeSaber.Configuration;
using Zenject;

namespace LoungeSaber.UI.BSML.Menu;

[ViewDefinition("LoungeSaber.UI.BSML.Menu.MissingMapsView.bsml")]
public class MissingMapsViewController : BSMLAutomaticViewController
{
    [Inject] private readonly PluginConfig _config = null;
    
    [CanBeNull] public event Action<bool> UserChoseToDownloadMaps;
    
    [UIValue("missingMapText")] private string MissingMapText { get; set; } = "placeholder";

    public void SetMissingMapCount(int missingMapCount)
    {
        MissingMapText = $"It looks like you are missing {missingMapCount} map(s).";
        NotifyPropertyChanged(nameof(MissingMapText));
    }
    
    [UIAction("yesButtonOnClick")] private void YesButtonOnClick() => UserChoseToDownloadMaps?.Invoke(true);
    [UIAction("noButtonOnClick")] private void NoButtonOnClick() => UserChoseToDownloadMaps?.Invoke(false);

    [UIValue("automaticallyDownloadNewMaps")]
    private bool AutomaticallyDownloadNewMaps
    {
        get => _config.DownloadMapsAutomatically;
        set => _config.DownloadMapsAutomatically = value;
    }
}