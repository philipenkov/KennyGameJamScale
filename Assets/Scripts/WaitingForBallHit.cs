using UnityEngine;

public class WaitingForBallHit : MonoBehaviour
{
    [SerializeField] private GameLoop gameLoop;
    [SerializeField] private CanonBall canonBall;

    private void Start()
    {
        canonBall.OnHit += HandleCanonBallHit;
    }
    
    private void HandleCanonBallHit()
    {
        gameLoop.GoToNextState();
    }

    private void OnDestroy()
    {
        canonBall.OnHit -= HandleCanonBallHit;
    }
}
