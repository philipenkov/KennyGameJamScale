using UnityEngine;

public class ShipHoverAnimation : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float speed = 1f;

    private Vector3 _initialLocalPosition;

    private void Awake()
    {
        if (image != null)
        {
            _initialLocalPosition = image.transform.localPosition;
        }
    }

    private void Update()
    {
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        if (!image)
            return;

        float t = Mathf.PingPong(Time.time * speed, 1f);

        float easedT = -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;

        Vector3 newPos = _initialLocalPosition;
        newPos.y += easedT * distance;
        
        image.transform.localPosition = newPos;
    }
}