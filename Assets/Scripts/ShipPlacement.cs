using System;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlaceDirection
{
    Up,
    Right
}

public class ShipPlacement : MonoBehaviour
{
    [SerializeField] private GlobalLoop globalLoop;
    [SerializeField] private EnemyShipPlacement enemyShipPlacement;
    [SerializeField] private ShipsCounter shipsCounter;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Board board;
    [SerializeField] private LayerMask boardLayer;

    [SerializeField] private FollowingShip smallShip;
    [SerializeField] private FollowingShip mediumShip;
    [SerializeField] private FollowingShip bigShip;
    
    [SerializeField] private PlacementConfig[] placementConfigs;

    private int _currentConfigId;
    private PlaceDirection _currentDirection = PlaceDirection.Up;
    private bool _isActive;
    private FollowingShip _currentShip;
    private List<Cell> _checkedCells = new List<Cell>();

    private void Start()
    {
        _isActive = true; //TODO: Заменить на вызов откуда-то
        ShowShipToPlace();
    }

    private void Update()
    {
        if (_isActive)
        {
            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            {
                _currentDirection = _currentDirection == PlaceDirection.Right ? PlaceDirection.Up : PlaceDirection.Right;
                _currentShip.Rotate(_currentDirection);
            }
            
            RaycastToField();
        }
    }

    private void RaycastToField()
    {
        Vector2 mousePosition = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, boardLayer))
        {
            if (_currentShip != null)
            {
                _currentShip.SetTargetPosition(hit.point);
            }

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                TryToPlace(hit.point);
            }
        }
    }

    private void TryToPlace(Vector3 position)
    {
        var cellPosition = board.WorldToCell(position);

        if (!ArePlacesFree(cellPosition, _currentDirection, placementConfigs[_currentConfigId],
                ref _checkedCells))
            return;

        Quaternion rotation;

        rotation = Quaternion.Euler(0, _currentDirection == PlaceDirection.Up ? 0 : 90, 0);

        GameObject realShipObject = Instantiate(placementConfigs[_currentConfigId].ShipPrefab, board.CellToWorld(cellPosition), rotation);
        IDamageable damageableShip = realShipObject.GetComponent<IDamageable>();
        damageableShip.SetHP(placementConfigs[_currentConfigId].NumberOfCells);
        
        foreach (var cell in _checkedCells)
        {
            cell.Occupy(damageableShip);
        }
        
        shipsCounter.AddShip(damageableShip);
        
        _currentConfigId++;
        ShowShipToPlace();
        
        if (_currentConfigId < placementConfigs.Length)
            return;
        
        enemyShipPlacement.PlaceEnemyShips();
        globalLoop.GoToNextState();
        _isActive = false;
    }

    private bool ArePlacesFree(Vector2Int startCellPosition, PlaceDirection direction, PlacementConfig config, ref List<Cell> checkedCells)
    {
        checkedCells.Clear();
        Vector2Int step = direction == PlaceDirection.Right ? Vector2Int.right : Vector2Int.up;
        int numberOfCells = config.NumberOfCells;

        for (int i = 0; i < numberOfCells; i++)
        {
            Vector2Int nextCellPosition = startCellPosition + step * i;
            Cell nextCell = board.GetCell(nextCellPosition);
                
            if (nextCell == null || nextCell.IsOccupied)
            {
                checkedCells.Clear();
                return false; //TODO: нотификатор, что нельзя поставить
            }
            checkedCells.Add(nextCell);
        }
        
        return true;
    }

    private void ShowShipToPlace()
    {
        if (_currentConfigId >= placementConfigs.Length)
        {
            TurnOffAllFollowingShips();
            return;
        }
        
        ShipType shipType = placementConfigs[_currentConfigId].ShipType;
        TurnOffAllFollowingShips();

        switch (shipType)
        {
            case ShipType.Small:
                _currentShip = smallShip;
                break;
            case ShipType.Medium:
                _currentShip = mediumShip;
                break;
            case ShipType.Big:
                _currentShip = bigShip;
                break;
        }
        
        _currentShip.SwitchFollowing(true);
        _currentShip.Rotate(_currentDirection);
    }

    private void TurnOffAllFollowingShips()
    {
        smallShip.SwitchFollowing(false);
        mediumShip.SwitchFollowing(false);
        bigShip.SwitchFollowing(false);
    }
}
