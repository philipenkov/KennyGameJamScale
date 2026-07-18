using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Transform origin;

    private Cell[,] _cells;

    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;

    private void Awake()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        _cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _cells[x, y] = new Cell(x, y);
            }
        }
    }

    public bool IsInside(Vector2Int cell)
    {
        return cell.x >= 0 &&
               cell.x < width &&
               cell.y >= 0 &&
               cell.y < height;
    }

    public Cell GetCell(Vector2Int position)
    {
        if (!IsInside(position))
            return null;

        return _cells[position.x, position.y];
    }

    public Vector2Int WorldToCell(Vector3 worldPosition)
    {
        Vector3 local = worldPosition - origin.position;

        int x = Mathf.FloorToInt(local.x / cellSize);
        int y = Mathf.FloorToInt(local.z / cellSize);

        return new Vector2Int(x, y);
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        return origin.position + new Vector3((cell.x + 0.5f) * cellSize, 0f, (cell.y + 0.5f) * cellSize);
    }

    public bool TryGetCell(Vector3 worldPosition, out Cell cell)
    {
        Vector2Int index = WorldToCell(worldPosition);

        if (!IsInside(index))
        {
            cell = null;
            return false;
        }

        cell = _cells[index.x, index.y];
        return true;
    }

    public void ResetBoard()
    {
        foreach (Cell cell in _cells)
            cell.Reset();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (origin == null)
            return;

        Gizmos.color = Color.white;

        for (int x = 0; x <= width; x++)
        {
            Vector3 start = origin.position + new Vector3(x * cellSize, 0, 0);
            Vector3 end = origin.position + new Vector3(x * cellSize, 0, height * cellSize);

            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= height; y++)
        {
            Vector3 start = origin.position + new Vector3(0, 0, y * cellSize);
            Vector3 end = origin.position + new Vector3(width * cellSize, 0, y * cellSize);

            Gizmos.DrawLine(start, end);
        }
    }

#endif
}