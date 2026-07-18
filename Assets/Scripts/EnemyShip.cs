using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] private GameObject model;

    public void SwitchModel(bool isOn)
    {
        model.SetActive(isOn);
    }
}
