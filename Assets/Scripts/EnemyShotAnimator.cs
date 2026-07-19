using System.Collections;
using UnityEngine;

public class EnemyShotAnimator : MonoBehaviour
{
    [SerializeField] private GameObject enemyCanonBall;
    [SerializeField] private float startYPosition = 10;
    [SerializeField] private Transform enemyShipsHolder;
    [SerializeField] private float startXOffset;
    [SerializeField] private float animationDuration = 1;
    [SerializeField] private Board board;

    public void PlayAnimationOnCell(Cell cell)
    {
        Transform enemyShipTransform = GetEnemyTransform();
        if (enemyShipTransform == null)
        {
            return;
        }
    
        Vector3 targetPosition = board.CellToWorld(cell.Position);
        
        Vector3 direction = enemyShipTransform.position - targetPosition;
        direction.y = 0f;
        direction = direction.normalized;

        Vector3 startPosition = targetPosition + direction * startXOffset;
        startPosition.y = startYPosition;

        StartCoroutine(PlayAnimation(startPosition, targetPosition));
        
    }
    
    private Transform GetEnemyTransform()
    {
        foreach (Transform enemyShip in enemyShipsHolder)
        {
            if (enemyShip.GetComponent<IDamageable>().HP > 0)
            {
                return enemyShip;
            }
        }
        return null;
    }

    private IEnumerator PlayAnimation(Vector3 startPosition, Vector3 targetPosition)
    {
        enemyCanonBall.transform.position = startPosition;
        enemyCanonBall.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float clamp = Mathf.Clamp01(elapsedTime / animationDuration);
            
            float easedClamp = clamp * clamp; 

            enemyCanonBall.transform.position = Vector3.Lerp(startPosition, targetPosition, easedClamp);
            yield return null;
        }

        enemyCanonBall.transform.position = targetPosition;
        enemyCanonBall.SetActive(false);
    }
}
