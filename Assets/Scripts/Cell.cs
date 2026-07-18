using UnityEngine;

public class Cell
{
    public Vector2Int Position { get; private set; }
    public bool IsOccupied { get; set; }

    public Cell(int x, int y)
    {
        Position = new Vector2Int(x, y);

        IsOccupied = false;
    }

    public void Occupy()
    {
        IsOccupied = true;
    }

    public void Reset()
    {
        IsOccupied = false;
    }
}