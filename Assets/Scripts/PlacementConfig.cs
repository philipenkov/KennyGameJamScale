using UnityEngine;

[CreateAssetMenu(fileName = "PlacementConfig", menuName = "Configs/PlacementConfig")]
public class PlacementConfig : ScriptableObject
{
    public int NumberOfCells;
    public ShipType ShipType;
}