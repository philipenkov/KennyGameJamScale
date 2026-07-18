using System.Collections.Generic;
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

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
            _cells[x, y] = new Cell(x, y);
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
        var local = worldPosition - origin.position;

        var x = Mathf.FloorToInt(local.x / cellSize);
        var y = Mathf.FloorToInt(local.z / cellSize);

        return new Vector2Int(x, y);
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        return origin.position + new Vector3((cell.x + 0.5f) * cellSize, 0f, (cell.y + 0.5f) * cellSize);
    }

    public bool TryGetCell(Vector3 worldPosition, out Cell cell)
    {
        var index = WorldToCell(worldPosition);

        if (!IsInside(index))
        {
            cell = null;
            return false;
        }

        cell = _cells[index.x, index.y];
        return true;
    }

    public Cell GetRandomFreeCell()
    {
        var freeCells = new List<Cell>();
        foreach (var cell in _cells)
            if (!cell.IsOccupied)
                freeCells.Add(cell);

        return freeCells[Random.Range(0, freeCells.Count)];
    }

    public List<Cell> GetFreeCellsSequence(int numberOfCells)
    {
        var totalValidCount = 0;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x <= width - numberOfCells; x++)
            {
                if (IsHorizontalSequenceFree(x, y, numberOfCells))
                    totalValidCount++;
            }
        }

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y <= height - numberOfCells; y++)
            {
                if (IsVerticalSequenceFree(x, y, numberOfCells))
                    totalValidCount++;
            }
        }

        if (totalValidCount == 0)
            return new List<Cell>();

        var targetIndex = Random.Range(0, totalValidCount);
        var currentIndex = 0;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x <= width - numberOfCells; x++)
            {
                if (!IsHorizontalSequenceFree(x, y, numberOfCells))
                    continue;
                if (currentIndex == targetIndex) 
                    return GetHorizontalSequence(x, y, numberOfCells);
                currentIndex++;
            }
        }

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y <= height - numberOfCells; y++)
            {
                if (!IsVerticalSequenceFree(x, y, numberOfCells))
                    continue;
                if (currentIndex == targetIndex) return GetVerticalSequence(x, y, numberOfCells);
                currentIndex++;
            }
        }

        return new List<Cell>();
    }

    private bool IsHorizontalSequenceFree(int startX, int y, int length)
    {
        for (var i = 0; i < length; i++)
        {
            if (_cells[startX + i, y].IsOccupied)
                return false;
        }
        
        return true;
    }

    private bool IsVerticalSequenceFree(int x, int startY, int length)
    {
        for (var i = 0; i < length; i++)
        {
            if (_cells[x, startY + i].IsOccupied)
                return false;
        }
        
        return true;
    }

    private List<Cell> GetHorizontalSequence(int startX, int y, int length)
    {
        var sequence = new List<Cell>(length);
        for (var i = 0; i < length; i++) 
        {
            sequence.Add(_cells[startX + i, y]);
        }
        return sequence;
    }

    private List<Cell> GetVerticalSequence(int x, int startY, int length)
    {
        var sequence = new List<Cell>(length);
        for (var i = 0; i < length; i++)
        {
            sequence.Add(_cells[x, startY + i]);
        }
        return sequence;
    }

    public void ResetBoard()
    {
        foreach (var cell in _cells)
            cell.Reset();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (origin == null)
            return;

        Gizmos.color = Color.white;

        for (var x = 0; x <= width; x++)
        {
            var start = origin.position + new Vector3(x * cellSize, 0, 0);
            var end = origin.position + new Vector3(x * cellSize, 0, height * cellSize);

            Gizmos.DrawLine(start, end);
        }

        for (var y = 0; y <= height; y++)
        {
            var start = origin.position + new Vector3(0, 0, y * cellSize);
            var end = origin.position + new Vector3(width * cellSize, 0, y * cellSize);

            Gizmos.DrawLine(start, end);
        }
    }

#endif
}