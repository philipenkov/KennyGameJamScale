using System;
using UnityEngine;

public class EnemyShip : MonoBehaviour, IDamageable
{
    public event Action<IDamageable> OnDeath;
    
    [SerializeField] private GameObject model;

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
        gameObject.SetActive(false);
        OnDeath?.Invoke(this);
    }
}
