using System;
using UnityEngine;

public enum ShipType
{
    Small,
    Medium,
    Big
}

public class Ship : MonoBehaviour
{
    [SerializeField] private GameObject hoveredAnimationObject;

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
}
