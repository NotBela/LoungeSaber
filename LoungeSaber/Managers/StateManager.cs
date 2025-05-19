using System;
using LoungeSaber.Models.Divisions;

namespace LoungeSaber.Managers
{
    public class StateManager
    {
        public State CurrentState { get; private set; } = State.Loading;

        public event Action<State> StateChanged;
        
        public void SwitchState(State state)
        {
            CurrentState = state;
            
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