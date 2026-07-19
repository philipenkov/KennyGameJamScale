using System;
using UnityEngine;

public class Cell
{
    public event Action<Cell> OnHit;
    public event Action<Cell> OnMissed;
    
    public Vector2Int Position { get; private set; }
    public bool IsOccupied { get; private set; }
    public bool IsEnemyCell { get; private set; }
    public bool IsHit => _isHit;
    public IDamageable ShipOnCell => _shipOnCell;
    
    private bool _isHit;
    private IDamageable _shipOnCell;

    public Cell(int x, int y)
    {
        Position = new Vector2Int(x, y);

        IsOccupied = false;
    }

    public void Occupy(IDamageable ship, bool isEnemyCell)
    {
        IsOccupied = true;
        _shipOnCell = ship;
        IsEnemyCell = isEnemyCell;
    }

    public void Reset()
    {
        IsOccupied = false;
        _shipOnCell = null;
    }

    public void HandleCanonBallHit()
    {
        if (IsOccupied && !_isHit)
        {
            _shipOnCell.TakeDamage();
            _isHit = true;
            OnHit?.Invoke(this);
        }
        else
        {
            OnMissed?.Invoke(this);
        }
    }
}