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
            await Task.Delay(unpauseTime - DateTime.UtcNow);
            
            _pauseController.HandlePauseMenuManagerDidPressContinueButton();
            StillInStartingPauseMenu = false;
        }
    }
}