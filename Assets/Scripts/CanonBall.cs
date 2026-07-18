using UnityEngine;

public class CanonBall : MonoBehaviour
{
    [SerializeField] private Board board;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            var cellPosition = board.WorldToCell(transform.position);
            Cell cell = board.GetCell(cellPosition);
            cell.HandleCanonBallHit();
            Debug.Log($"Placed on Water and got {cellPosition}");
            gameObject.SetActive(false);
        }
    }
}
