﻿using LoungeSaber.UI.FlowCoordinators;
using LoungeSaber.UI.ViewManagers;
using SiraUtil.Affinity;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.AffinityPatches.MenuPatches;

public class BeatmapDifficultySegmentedControlPatch : IAffinity
{
    [Inject] private readonly StandardLevelDetailViewManager _standardLevelDetailViewManager = null;
    [Inject] private readonly MatchFlowCoordinator _matchFlowCoordinator = null;
    
    [AffinityPatch(typeof(BeatmapDifficultySegmentedControlController),
        nameof(BeatmapDifficultySegmentedControlController.SetData))]
    [AffinityPostfix]
    private void Postfix(BeatmapDifficultySegmentedControlController __instance)
    {
        if (!_matchFlowCoordinator.isActivated) 
            return;
        
        var texts = __instance._difficultySegmentedControl._texts;
        
        __instance._difficultySegmentedControl.SetTexts(texts.Select(i => $"{i}\n(Category: {_standardLevelDetailViewManager.CurrentVotingMap.Category})").ToList());
    }
}