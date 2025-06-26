using SiraUtil.Affinity;
using Zenject;

namespace LoungeSaber.AffinityPatches
{
    public class LevelBarSetupPatch : IAffinity
    {
        [Inject] private readonly PauseMenuManager _pauseMenuManager = null;
        
        [AffinityPatch(typeof(LevelBar), nameof(LevelBar.SetupData))]
        [AffinityPostfix]
        private void Postfix(LevelBar __instance)
        {
            _pauseMenuManager._backButton.gameObject.SetActive(false);
            _pauseMenuManager._continueButton.gameObject.SetActive(false);
            _pauseMenuManager._restartButton.gameObject.SetActive(false);

            __instance._songNameText.text = "Starting Soon...";
            __instance._authorNameText.text = "Get ready!";
        }
    }
}