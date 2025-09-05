using LoungeSaber.Game;
using SiraUtil.Affinity;
using Zenject;

namespace LoungeSaber.AffinityPatches.PausePatches
{
    public class PausePatch : IAffinity
    {
        [Inject] private readonly MatchStartUnpauseController _matchStartUnpauseController = null;
        
        [AffinityPrefix]
        [AffinityPatch(typeof(PauseController), nameof(PauseController.Pause))]
        private bool Prefix() => _matchStartUnpauseController.StillInStartingPauseMenu;
    }
}