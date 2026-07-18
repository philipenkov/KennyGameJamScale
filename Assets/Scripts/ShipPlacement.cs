using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlaceDirection
{
    Up,
    Right
}

public class ShipPlacement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Board board;
    [SerializeField] private LayerMask boardLayer;

    [SerializeField] private GameObject smallShip;
    [SerializeField] private GameObject mediumShip;
    [SerializeField] private GameObject bigShip;
    
    [SerializeField] private PlacementConfig[] placementConfigs;

    private int _currentConfigId;
    private PlaceDirection _currentDirection = PlaceDirection.Right;
    private bool _isActive;

    private void Start()
    {
        _isActive = true; //TODO: Заменить на вызов откуда-то
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            _currentDirection = _currentDirection == PlaceDirection.Right ? PlaceDirection.Up : PlaceDirection.Right;
        }
        
        if (_isActive)
            RaycastToField();
    }

    private void RaycastToField()
    {
        Vector2 mousePosition = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, boardLayer))
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                TryToPlace(hit.point);
            }
        }
    }

    private void TryToPlace(Vector3 position)
    {
        var cellPosition = board.WorldToCell(position);

        if (ArePlacesFree(cellPosition, _currentDirection, placementConfigs[_currentConfigId]))
        {
            Debug.Log("===PLACED");
            _currentConfigId++;
            if (_currentConfigId >= placementConfigs.Length)
            {
                //TODO: закончилась фаза расстановки
                return;
            }
        }
        else
        {
            Debug.Log("===NOT PLACED");
        }
    }

    private bool ArePlacesFree(Vector2Int startCellPosition, PlaceDirection direction, PlacementConfig config)
    {
        Cell startCell = board.GetCell(startCellPosition);

        if (startCell == null || startCell.IsOccupied)
        {
            return false; //TODO: нотификатор, что нельзя поставить
        }
        
        int numberOfCells = config.NumberOfCells;
        if (numberOfCells == 1)
        {
            return true;
        }

        if (direction == PlaceDirection.Right)
        {
            for (int i = 0; i < numberOfCells; i++)
            {
                Vector2Int nextCellPosition = startCellPosition + new Vector2Int(i, 0);
                Cell nextCell = board.GetCell(nextCellPosition);
                if (nextCell == null || nextCell.IsOccupied)
                {
                    return false;
                }
            }
            
            return true;
        }
        else
        {
            for (int i = 0; i < numberOfCells; i++)
            {
                Vector2Int nextCellPosition = startCellPosition + new Vector2Int(0, i);
                Cell nextCell = board.GetCell(nextCellPosition);
                if (nextCell == null || nextCell.IsOccupied)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
