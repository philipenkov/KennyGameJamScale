using System;
using System.Collections;
using UnityEngine;

public class EnemyShip : MonoBehaviour, IDamageable
{
    public event Action<IDamageable> OnDeath;
    
    [SerializeField] private ShipSinkAnimator shipSinkAnimator;
    [SerializeField] private GameObject model;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioSource sinkSource;

    public int HP => _hp;
    private int _hp;

    public void SwitchModel(bool isOn)
    {
        model.SetActive(isOn);
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
        model.SetActive(true);
        StartCoroutine(PlaySinkAnimation());
        OnDeath?.Invoke(this);
    }
    
    private IEnumerator PlaySinkAnimation()
    {
        sinkSource.Play();
        shipSinkAnimator.PlayAnimation();
        yield return new WaitForSeconds(7f);
        gameObject.SetActive(false);
        model.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
