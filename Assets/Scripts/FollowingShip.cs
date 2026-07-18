using UnityEngine;
using UnityEngine.InputSystem;

public class FollowingShip : MonoBehaviour
{
    [SerializeField] private GameObject shipModel;
    [SerializeField] private float speed = 100;
    
    private bool _isFollowing;
    private Vector3 _targetPosition;
    
    public void SwitchFollowing(bool isFollowing)
    {
        _isFollowing = isFollowing;
        shipModel.SetActive(_isFollowing);
    }

    public void SetTargetPosition(Vector3 position)
    {
        _targetPosition = position;
    }

    private void Update()
    {
        if (!_isFollowing)
            return;

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * speed);
    }
    
    public void Rotate(PlaceDirection direction)
    {
        switch (direction)
        {
            case PlaceDirection.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case PlaceDirection.Right:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
        }
    }
}