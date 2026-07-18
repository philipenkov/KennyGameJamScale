using UnityEngine;

public class CanonBall : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Rigidbody rb;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            board.HandleCanonBallHit(transform.position);
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
