using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using Zenject;

namespace LoungeSaber.UI.BSML.Settings;

[ViewDefinition("LoungeSaber.UI.BSML.Settings.LoungeSaberSettingsView.bsml")]
public class LoungeSaberSettingsViewController : BSMLAutomaticViewController, IInitializable, IDisposable
{
    [Inject] private readonly BSMLSettings _bsmlSettings = null;
    
    public void Initialize()
    {
        _bsmlSettings.AddSettingsMenu("LoungeSaber", "LoungeSaber.UI.BSML.Settings.LoungeSaberSettingsView.bsml", this);
    }

    public void Dispose()
    {
        _bsmlSettings.RemoveSettingsMenu("LoungeSaber");
    }
}