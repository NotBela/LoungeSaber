using System;
using LoungeSaber.Models.Divisions;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Managers
{
    public class StateManager
    {
        [Inject] private readonly SiraLog _siraLog = null;
        
        public State CurrentState { get; private set; } = State.Loading;

        public event Action<State> StateChanged;
        
        public void SwitchState(State state)
        {
            CurrentState = state;
            
            _siraLog.Info(state.ToString());
            
            StateChanged?.Invoke(state);
        }

        public void JoinRoom(Division division)
        {
            
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