using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.BSML.PauseMenu;

[ViewDefinition("LoungeSaber.UI.BSML.PauseMenu.PauseMenuView.bsml")]
public class PauseMenuViewController : BSMLAutomaticViewController, IInitializable, IDisposable
{
    [Inject] private readonly PauseController _pauseController = null;
    
    private readonly FloatingScreen _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(50f, 50f), false, new Vector3(0f, .5f, 1f), Quaternion.identity);
    
    public void Initialize()
    {
        _pauseController._gamePause.didResumeEvent += Resumed;
        _pauseController._gamePause.didPauseEvent += Paused;
        
        _floatingScreen.SetRootViewController(this, AnimationType.None);
        _floatingScreen.gameObject.SetActive(true);
    }

    private void Paused() => _floatingScreen.gameObject.SetActive(true);

    private void Resumed() => _floatingScreen.gameObject.SetActive(false);

    public void Dispose()
    {
        _pauseController._gamePause.didResumeEvent -= Resumed;
        _pauseController._gamePause.didPauseEvent -= Paused;
    }
}