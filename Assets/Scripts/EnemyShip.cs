using System;
using System.Collections;
using UnityEngine;

public class EnemyShip : MonoBehaviour, IDamageable
{
    public event Action<IDamageable> OnDeath;
    
    [SerializeField] private ShipSinkAnimator shipSinkAnimator;
    [SerializeField] private GameObject model;

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
        shipSinkAnimator.PlayAnimation();
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
