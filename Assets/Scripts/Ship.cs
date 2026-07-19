using System;
using System.Collections;
using UnityEngine;

public interface IDamageable
{
    event Action<IDamageable> OnDeath;
    int HP { get; }
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
    
    [SerializeField] private ShipSinkAnimator shipSinkAnimator;
    [SerializeField] private GameObject hoveredAnimationObject;
    [SerializeField] private Transform playerSpawnTransform;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioSource sinkSource;
    
    public Transform PlayerSpawnTransform => playerSpawnTransform;
    public int HP => _hp;

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
        hitSource.Play();
        if (_hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        StartCoroutine(PlaySinkAnimation());
        OnDeath?.Invoke(this);
    }

    private IEnumerator PlaySinkAnimation()
    {
        sinkSource.Play();
        shipSinkAnimator.PlayAnimation();
        yield return new WaitForSeconds(7f);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
