using System;
using UnityEngine;

public interface IDamageable
{
    event Action<IDamageable> OnDeath;
    void SetHP(int hp);
    void TakeDamage();
    void Die();
}

public enum ShipType
{
    Small,
    Medium,
    Big
}

public class Ship : MonoBehaviour, IDamageable
{
    public event Action<IDamageable> OnDeath;
    
    [SerializeField] private GameObject hoveredAnimationObject;
    [SerializeField] private Transform playerSpawnTransform;
    
    public Transform PlayerSpawnTransform => playerSpawnTransform;

    private int _hp;

    private void Awake()
    {
        ResetHoveredAnimation();
    }

    public void PlayHoveredAnimation()
    {
        hoveredAnimationObject.SetActive(true);
    }

    public void ResetHoveredAnimation()
    {
        hoveredAnimationObject.SetActive(false);
    }
    
    public void SetHP(int hp)
    {
        _hp = hp;
    }

    public void TakeDamage()
    {
        _hp--;
        if (_hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        OnDeath?.Invoke(this);
    }
}
