using System.Collections;
using UnityEngine;

public class CellHitAnimator : MonoBehaviour
{
    [SerializeField] private WreckageSpawner wreckageSpawner;
    [SerializeField] private GameObject splashVFXObject;
    [SerializeField] private Board board;

    private Cell[,] _subscribedCells;

    private void Start()
    {
        splashVFXObject.SetActive(false);
        board.OnCellsCreated += HandleCellsCreated;
    }

    private void HandleCellsCreated(Cell[,] cells)
    {
        _subscribedCells = cells;
        SubscribeToCellHit();
    }

    private void SubscribeToCellHit()
    {
        foreach (var cell in _subscribedCells)
        {
            cell.OnHit += PlayHitAnimationOnCell;
            cell.OnMissed += PlayMissedAnimationOnCell;
        }
    }
    
    private void PlayMissedAnimationOnCell(Cell cell)
    {
        Vector3 spawnPosition = board.CellVectorToWorldPosition(cell.Position);
        splashVFXObject.transform.position = spawnPosition;
        splashVFXObject.SetActive(true);
        StartCoroutine(DisableSplashes());
    }

    private void PlayHitAnimationOnCell(Cell cell)
    {
        Vector3 spawnPosition = board.CellVectorToWorldPosition(cell.Position);
        splashVFXObject.transform.position = spawnPosition;
        splashVFXObject.SetActive(true);
        StartCoroutine(DisableSplashes());
        wreckageSpawner.SpawnOnPosition(spawnPosition, 1);
    }

    private IEnumerator DisableSplashes()
    {
        yield return new WaitForSeconds(1.5f);
        splashVFXObject.SetActive(false);
    }

    private void OnDestroy()
    {
        board.OnCellsCreated -= HandleCellsCreated;
        
        foreach (var cell in _subscribedCells)
        {
            cell.OnHit -= PlayHitAnimationOnCell;
            cell.OnMissed -= PlayMissedAnimationOnCell;
        }
    }
}