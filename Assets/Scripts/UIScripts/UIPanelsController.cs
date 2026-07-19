using System;
using StateMachine;
using UnityEngine;

namespace UIScripts
{
    public class UIPanelsController : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private GlobalLoop globalLoop;

        [SerializeField] private HintPanel placeShips;
        [SerializeField] private HintPanel selectShip;
        [SerializeField] private HintPanel aim;
        [SerializeField] private HintPanel enemyTurn;

        private bool _isAllowed = true;
        private bool _madeLoop;

        private void Start()
        {
            gameLoop.GameStateChanged += HandleGameState;
            globalLoop.GlobalStateChanged += HandleGlobalState;
            HandleGlobalState(GlobalState.ShipPlacement);
        }

        private void HandleGameState(GameState gameState)
        {
            if (!_isAllowed)
                return;

            if (_madeLoop)
            {
                enemyTurn.Hide();
                _isAllowed = false;
                return;
            }
            
            switch (gameState)
            {
                case GameState.ShipSelect:
                    placeShips.Hide();
                    enemyTurn.Hide();
                    selectShip.Show();
                    break;
                case GameState.Shooting:
                    selectShip.Hide();
                    aim.Show();
                    break;
                case GameState.EnemyTurn:
                    aim.Hide();
                    enemyTurn.Show();
                    _madeLoop = true;
                    break;
            }
        }

        private void HandleGlobalState(GlobalState globalState)
        {
            if (!_isAllowed)
                return;

            switch (globalState)
            {
                case GlobalState.ShipPlacement:
                    enemyTurn.Hide();
                    placeShips.Show();
                    break;
            }
        }

        private void TurnOffHints()
        {
            _isAllowed = false;
        }

        private void OnDestroy()
        {
            gameLoop.GameStateChanged -= HandleGameState;
            globalLoop.GlobalStateChanged -= HandleGlobalState;
        }
    }
}
