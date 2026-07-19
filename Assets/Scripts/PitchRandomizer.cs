using System;
using UnityEngine;

public class PitchRandomizer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }
}
