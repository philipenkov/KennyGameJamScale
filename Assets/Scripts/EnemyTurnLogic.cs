using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyTurnLogic : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private GameLoop gameLoop;
    [SerializeField] private float enemyTurnDelay = 2;
    [SerializeField] private float canonBallAnimationDelay = 1;

    private WaitForSeconds _delay;
    private WaitForSeconds _canonBallAnimationDelay;

    private IDamageable _currentTarget;
    private readonly List<Cell> _currentTargetHits = new List<Cell>();
    private readonly Queue<Cell> _pendingShipCells = new Queue<Cell>();

    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };
    
    private void Start()
    {
        _delay = new WaitForSeconds(enemyTurnDelay);
        _canonBallAnimationDelay = new WaitForSeconds(canonBallAnimationDelay);
        gameLoop.GameStateChanged += HandleGameStateChange;
    }
    
    private void HandleGameStateChange(GameState newState)
    {
        if (newState != GameState.EnemyTurn)
            return;
        
        DoTurn();
    }

    public void DoTurn()
    {
        Cell targetCell = ChooseTargetCell();
        StartCoroutine(DoHit(targetCell));
    }
    
    private IEnumerator DoHit(Cell targetCell)
    {
        yield return _delay;
        //TODO: анимация выстрела
        yield return _canonBallAnimationDelay;

        IDamageable ship = targetCell.ShipOnCell;

        targetCell.HandleCanonBallHit();
        board.RemoveCellFromNonEnemyCache(targetCell);

        if (targetCell.IsOccupied && ship != null)
        {
            RegisterHit(targetCell, ship);
        }
        
        gameLoop.GoToNextState();
    }

    private void RegisterHit(Cell hitCell, IDamageable ship)
    {
        if (ship.HP <= 0)
        {
            if (ship == _currentTarget)
            {
                _currentTarget = null;
                _currentTargetHits.Clear();
            }
            return;
        }

        if (_currentTarget == null)
        {
            _currentTarget = ship;
            _currentTargetHits.Clear();
            _currentTargetHits.Add(hitCell);
        }
        else if (ship == _currentTarget)
        {
            _currentTargetHits.Add(hitCell);
        }
        else
        {
            _pendingShipCells.Enqueue(hitCell);
        }
    }
    
    private Cell ChooseTargetCell()
    {
        if (_currentTarget == null)
        {
            TakeNextPendingShip();
        }

        if (_currentTarget != null)
        {
            Cell nextCell = GetNextCellAroundTarget();
            if (nextCell != null)
                return nextCell;

            _currentTarget = null;
            _currentTargetHits.Clear();
        }

        return board.GetRandomNonEnemyCell();
    }

    private void TakeNextPendingShip()
    {
        while (_pendingShipCells.Count > 0)
        {
            Cell cell = _pendingShipCells.Dequeue();
            IDamageable ship = cell.ShipOnCell;

            if (ship != null && ship.HP > 0)
            {
                _currentTarget = ship;
                _currentTargetHits.Clear();
                _currentTargetHits.Add(cell);
                return;
            }
        }
    }

    private Cell GetNextCellAroundTarget()
    {
        var candidates = new List<Cell>();

        if (_currentTargetHits.Count == 1)
        {
            Vector2Int origin = _currentTargetHits[0].Position;
            foreach (Vector2Int direction in Directions)
            {
                TryAddCandidate(origin + direction, candidates);
            }
        }
        else
        {
            Vector2Int min = _currentTargetHits[0].Position;
            Vector2Int max = min;
            foreach (Cell hit in _currentTargetHits)
            {
                min = Vector2Int.Min(min, hit.Position);
                max = Vector2Int.Max(max, hit.Position);
            }

            Vector2Int axis = max.x > min.x ? Vector2Int.right : Vector2Int.up;
            TryAddCandidate(min - axis, candidates);
            TryAddCandidate(max + axis, candidates);
        }

        if (candidates.Count == 0)
            return null;

        return candidates[Random.Range(0, candidates.Count)];
    }

    private void TryAddCandidate(Vector2Int position, List<Cell> candidates)
    {
        Cell cell = board.GetCell(position);
        if (cell != null && !cell.IsHit && !cell.IsEnemyCell)
            candidates.Add(cell);
    }

    private void OnDestroy()
    {
        gameLoop.GameStateChanged -= HandleGameStateChange;
    }
}

