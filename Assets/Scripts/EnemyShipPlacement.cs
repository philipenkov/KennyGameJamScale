using System.Collections.Generic;
using UnityEngine;

public class EnemyShipPlacement : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private PlacementConfig[] placementConfigs;

    public void PlaceEnemyShips()
    {
        foreach (PlacementConfig placementConfig in placementConfigs)
        {
            int numberOfCells = placementConfig.NumberOfCells;
            GameObject enemyShip;

            if (numberOfCells == 1)
            {
                Cell cell = board.GetRandomFreeCell();
                cell.Occupy(true);
                enemyShip = Instantiate(placementConfig.ShipPrefab, board.CellToWorld(cell.Position), Quaternion.identity);
                continue;
            }
            
            List<Cell> freeCells = board.GetFreeCellsSequence(numberOfCells);
            foreach (Cell cell in freeCells)
            {
                cell.Occupy(true);
            }
            
            Quaternion rotation;
            rotation = Quaternion.Euler(0, freeCells[1].Position.x > freeCells[0].Position.x ? 90 : 0, 0);
            enemyShip = Instantiate(placementConfig.ShipPrefab, board.CellToWorld(freeCells[0].Position), rotation);
        }
    }
}