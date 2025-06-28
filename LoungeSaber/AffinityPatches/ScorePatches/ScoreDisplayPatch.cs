﻿using System;
using SiraUtil.Affinity;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.AffinityPatches.ScorePatches
{
    public class ScoreDisplayPatch : IAffinity
    {
        [Inject] private readonly ScoreController _scoreController = null;
        
        [AffinityPatch(typeof(ScoreUIController), nameof(ScoreUIController.UpdateScore), argumentTypes: new Type[] {typeof(int)})]
        [AffinityPrefix]
        private void Prefix(ref int displayScore)
        {
            displayScore = _scoreController._multipliedScore;
        }
    }
}