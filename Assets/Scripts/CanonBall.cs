using System;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
    public event Action OnHit; 
    
    [SerializeField] private Board board;
    [SerializeField] private Rigidbody rb;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            board.HandleCanonBallHit(transform.position);
            gameObject.SetActive(false);
            OnHit?.Invoke();
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            gameObject.SetActive(false);
            OnHit?.Invoke();
        }
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
