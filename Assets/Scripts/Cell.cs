using UnityEngine;

public class Cell
{
    public Vector2Int Position { get; private set; }
    public bool IsOccupied { get; private set; }
    public bool IsEnemyCell { get; private set; }

    public Cell(int x, int y)
    {
        Position = new Vector2Int(x, y);

        IsOccupied = false;
    }

    public void Occupy(bool isEnemyCell)
    {
        IsOccupied = true;
        IsEnemyCell = isEnemyCell;
    }

    public void Reset()
    {
        IsOccupied = false;
    }

    public void HandleCanonBallHit()
    {
        if (IsOccupied && IsEnemyCell)
        {
            Debug.Log("HIT");
        }
    }
}