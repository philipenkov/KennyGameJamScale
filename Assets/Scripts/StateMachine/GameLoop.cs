using System;
using StateMachine;
using UnityEngine;

public enum GameState
{
    ShipSelect,
    Shooting,
    ShootingResult,
    EnemyTurn
}

public class GameLoop : MonoBehaviour
{
    public event Action<GameState> GameStateChanged;
    
    public bool IsActive { get; private set; }
    
    [SerializeField] private GlobalLoop globalLoop;
    
    private GameState _currentState;

    private void Start()
    {
        _currentState = GameState.ShipSelect;
        globalLoop.GlobalStateChanged += SwitchActivation;
    }

    private void SwitchActivation(GlobalState newState)
    {
        if (newState == GlobalState.Game)
        {
            IsActive = true;
            GameStateChanged?.Invoke(_currentState);
        }
        else
        {
            IsActive = false;
        }
    }

    public void GoToNextState()
    {
        if (!IsActive)
            return;
        
        int stateCount = Enum.GetValues(typeof(GameState)).Length;
        _currentState = (GameState)(((int)_currentState + 1) % stateCount);
        GameStateChanged?.Invoke(_currentState);
    }

    private void OnDestroy()
    {
        globalLoop.GlobalStateChanged -= SwitchActivation;
    }
}
