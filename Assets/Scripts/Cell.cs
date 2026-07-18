using UnityEngine;

public class Cell
{
    public Vector2Int Position { get; private set; }
    public bool IsOccupied { get; private set; }

    private bool _isHit;
    private IDamageable _shipOnCell;

    public Cell(int x, int y)
    {
        Position = new Vector2Int(x, y);

        IsOccupied = false;
    }

    public void Occupy(IDamageable ship)
    {
        IsOccupied = true;
        _shipOnCell = ship;
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
            Debug.Log("HIT");
            _isHit = true;
        }
    }
}