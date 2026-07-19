using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShipSinkAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    public void PlayAnimation()
    {
        _animator.enabled = true;
    }
}
