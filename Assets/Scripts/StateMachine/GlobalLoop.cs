using System;
using UnityEngine;

namespace StateMachine
{
    public enum GlobalState
    {
        ShipPlacement,
        Game,
        Results
    }
    
    public class GlobalLoop : MonoBehaviour
    {
        public event Action<GlobalState> GlobalStateChanged;
        
        private GlobalState _currentGlobalState;

        private void Start()
        {
            _currentGlobalState = GlobalState.ShipPlacement;
        }

        public void GoToNextState()
        {
            int stateCount = Enum.GetValues(typeof(GlobalState)).Length;
            _currentGlobalState = (GlobalState)(((int)_currentGlobalState + 1) % stateCount);
            GlobalStateChanged?.Invoke(_currentGlobalState);
        }
    }
}