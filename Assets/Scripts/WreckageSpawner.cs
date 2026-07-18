using System.Collections;
using UnityEngine;

public class WreckageSpawner : MonoBehaviour
{ 
    [SerializeField] private GameObject wreckagePrefab;
    [SerializeField] private Vector3 startOffset;

    public void SpawnOnPosition(Vector3 position)
    {
        Vector3 startPos = position + startOffset;
        GameObject wreckage = Instantiate(wreckagePrefab, startPos, Quaternion.identity);
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(0.5f);
    }
}