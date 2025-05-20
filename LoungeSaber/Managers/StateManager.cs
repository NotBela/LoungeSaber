using System;
using System.Threading.Tasks;
using LoungeSaber.Models.Divisions;
using LoungeSaber.Server.MatchRoom;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Managers
{
    public class StateManager
    {
        [Inject] private readonly SiraLog _siraLog = null;
        [Inject] private readonly LoungeServerInterfacer _loungeServerInterfacer = null;
        
        public State CurrentState { get; private set; } = State.Loading;

        public event Action<State> StateChanged;
        
        public void SwitchState(State state)
        {
            CurrentState = state;
            
            _siraLog.Info(state.ToString());
            
            StateChanged?.Invoke(state);
        }

        public async Task JoinRoom(Division division)
        {
            SwitchState(State.Loading);
            await _loungeServerInterfacer.ConnectToLoungeServer(division);
        }
        
        public enum State
        {
            Loading,
            DivisionSelector,
            InMatch,
            MatchLobby
        }
    }
}