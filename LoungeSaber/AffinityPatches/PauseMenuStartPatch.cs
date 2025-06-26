using SiraUtil.Affinity;

namespace LoungeSaber.AffinityPatches
{
    public class PauseMenuStartPatch : IAffinity
    {
        [AffinityPatch(typeof(PauseMenuManager), nameof(PauseMenuManager.Start))]
        [AffinityPostfix]
        private void Postfix(PauseMenuManager __instance)
        {
            __instance._restartButton.gameObject.SetActive(false);
        }
    }
}