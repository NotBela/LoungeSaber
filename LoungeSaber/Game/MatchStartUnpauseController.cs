using System;
using System.Threading.Tasks;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Game
{
    public class MatchStartUnpauseController
    {
        [Inject] private readonly PauseController _pauseController = null;
        
        public bool StillInStartingPauseMenu { get; private set; } = true;

        public async Task UnpauseLevelAtTime(DateTime unpauseTime)
        {
            await Task.Delay((int) Math.Max(0, (unpauseTime - DateTime.UtcNow).TotalMilliseconds));
            
            _pauseController.HandlePauseMenuManagerDidPressContinueButton();
            StillInStartingPauseMenu = false;
        }
    }
}