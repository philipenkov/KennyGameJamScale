using System.Collections;
using UnityEngine;

public class ShipSinkAnimator : MonoBehaviour
{
    public void PlayAnimation()
    {
        StartCoroutine(SinkRoutine());
    }

    private IEnumerator SinkRoutine()
    {
        float rotateDuration = 0.5f;
        float sinkDuration = 1.0f;

        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        Quaternion targetRot = startRot * Quaternion.Euler(30f, 0f, 0f);
        Vector3 targetPos = startPos + new Vector3(0f, -15f, 0f);

        float elapsed = 0f;
        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotateDuration;
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        transform.localRotation = targetRot;

        elapsed = 0f;
        while (elapsed < sinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / sinkDuration;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        transform.localPosition = targetPos;
    }
}