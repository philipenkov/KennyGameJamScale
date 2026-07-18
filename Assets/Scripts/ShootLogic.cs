using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootLogic : MonoBehaviour
{
    public event Action OnShot;
    
    [SerializeField] private GameLoop gameLoop;
    [SerializeField] private GameObject canonBall;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shotPower;
    
    private bool _isActive;
    private bool _hasShot;
    private Rigidbody _ballRb;

    private void Awake()
    {
        gameLoop.GameStateChanged += HandleGameStateChange;
        _ballRb = canonBall.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
            OnShot?.Invoke();
        }
    }

    private void HandleGameStateChange(GameState newState)
    {
        if (newState != GameState.Shooting)
        {
            _isActive = false;
            return;
        }
        
        _isActive = true;
        _hasShot = false;
    }

    private void Shoot()
    {
        if (_hasShot)
            return;
        
        _hasShot = true;
        _isActive = false;
        gameLoop.GoToNextState();

        StartCoroutine(DelayBeforeShot());
    }

    private IEnumerator DelayBeforeShot()
    {
        yield return new WaitForSeconds(2f);
        canonBall.transform.position = firePoint.position;
        canonBall.transform.rotation = firePoint.rotation;
        canonBall.SetActive(true);
        if (_ballRb != null)
        {
            _ballRb.linearVelocity = firePoint.forward * shotPower;
        }
    }

    private void OnDestroy()
    {
        gameLoop.GameStateChanged -= HandleGameStateChange;
    }
}
