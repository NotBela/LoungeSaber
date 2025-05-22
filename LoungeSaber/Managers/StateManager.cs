using System;
using System.Threading.Tasks;
using LoungeSaber.Models.Divisions;
using LoungeSaber.Server.Api;
using LoungeSaber.Server.MatchRoom;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Managers
{
    public class StateManager
    {
        [Inject] private readonly LoungeServerInterfacer _loungeServerInterfacer = null;

        public async Task JoinRoom(Division division)
        {
            await _loungeServerInterfacer.ConnectToLoungeServer(division);
        }
    }
}