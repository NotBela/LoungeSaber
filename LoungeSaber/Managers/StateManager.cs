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
        [Inject] private readonly SiraLog _siraLog = null;
        [Inject] private readonly LoungeServerInterfacer _loungeServerInterfacer = null;
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        
        public event Action OnDivisionDataRefreshed;

        public async Task JoinRoom(Division division)
        {
            await _loungeServerInterfacer.ConnectToLoungeServer(division);
        }

        public async Task RequestDivisionDataRefresh()
        {
            await _loungeSaberApi.FetchDivisions();
            OnDivisionDataRefreshed?.Invoke();
        }
    }
}