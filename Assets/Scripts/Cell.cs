using UnityEngine;

public class Cell
{
    public Vector2Int Position { get; private set; }
    public bool HasShip { get; set; }

    public Cell(int x, int y)
    {
        Position = new Vector2Int(x, y);

        HasShip = false;
    }

    public void PlaceShip()
    {
        HasShip = true;
    }

    public void Reset()
    {
        HasShip = false;
    }
}