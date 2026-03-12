using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animatorOverlay;
    private Animator _animator;
    private Rigidbody2D _rigidbody;

    readonly int _hashSpeed = Animator.StringToHash("Speed");
    readonly int _hashJump = Animator.StringToHash("Jump");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetFloat(_hashSpeed, Mathf.Abs(_rigidbody.linearVelocityX));
    }

    public void JumpAnimation()
    {
        _animator.SetTrigger(_hashJump);
    }

    public void AttackAnimation(string weaponName)
    {
        _animator.SetTrigger(weaponName + "Attack");
        _animatorOverlay.SetTrigger(weaponName + "Attack");
    }
}
