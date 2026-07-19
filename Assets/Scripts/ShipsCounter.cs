using System;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class ShipsCounter : MonoBehaviour
{
    public event Action OnLost;
    public event Action OnWon;

    [SerializeField] private GlobalLoop globalLoop;
    
    private int _playerShips;
    private int _enemyShips;
    private List<IDamageable> _ships = new List<IDamageable>();

    public void AddShip(IDamageable ship)
    {
        _ships.Add(ship);
        
        if (ship is Ship)
            _playerShips++;
        else
            _enemyShips++;
        
        ship.OnDeath += RegisterShipDeath;
    }

    private void RegisterShipDeath(IDamageable diedShip)
    {
        if (diedShip is Ship)
        {
            _playerShips--;
            if (_playerShips <= 0)
            {
                OnLost?.Invoke();
                Debug.Log("Player lost");
                globalLoop.GoToNextState();
            }
        }
        else if (diedShip is EnemyShip)
        {
            _enemyShips--;
            if (_enemyShips <= 0)
            {
                OnWon?.Invoke();
                Debug.Log("Player Won");
                globalLoop.GoToNextState();
            }
        }
    }

    private void OnDestroy()
    {
        if (_ships.Count > 0)
        {
            foreach (IDamageable ship in _ships)
            {
                ship.OnDeath -= RegisterShipDeath;
            }
        }
    }
}