using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootLogic : MonoBehaviour
{
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
        
        canonBall.transform.position = firePoint.position;
        canonBall.transform.rotation = firePoint.rotation;
        canonBall.SetActive(true);
        if (_ballRb != null)
        {
            _ballRb.linearVelocity = firePoint.forward * shotPower;
        }
        
        _hasShot = true;
    }

    private void OnDestroy()
    {
        gameLoop.GameStateChanged -= HandleGameStateChange;
    }
}
