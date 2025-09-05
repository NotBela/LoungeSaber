using LoungeSaber.Game;
using SiraUtil.Affinity;
using Zenject;

namespace LoungeSaber.AffinityPatches.PausePatches;

public class ContinuePausePatch : IAffinity
{
    [Inject] private readonly MatchStartUnpauseController _matchStartUnpauseController = null;

    [AffinityPatch(typeof(PauseController), nameof(PauseController.HandlePauseMenuManagerDidPressContinueButton))]
    [AffinityPrefix]
    private bool Prefix() => !_matchStartUnpauseController.StillInStartingPauseMenu;
}