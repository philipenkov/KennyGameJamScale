using System.Collections;
using UnityEngine;

public class WreckageSpawner : MonoBehaviour
{ 
    [SerializeField] private GameObject wreckagePrefab;
    [SerializeField] private Vector3 startOffset;
    [SerializeField] private float animationDuration = 1;

    public void SpawnOnPosition(Vector3 position, float delay)
    {
        Vector3 startPos = position + startOffset;
        GameObject wreckage = Instantiate(wreckagePrefab, startPos, Quaternion.identity);
        StartCoroutine(PlayAnimation(wreckage.transform, delay, startPos, position));
    }

    private IEnumerator PlayAnimation(Transform wreckageTransform, float delay, Vector3 startPosition, Vector3 targetPosition)
    {
        yield return new WaitForSeconds(delay);
        
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            
            wreckageTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        wreckageTransform.position = targetPosition;
    }
}