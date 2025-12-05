using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animatorOverlay;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Vector3 _firstScale;

    readonly int _hashSpeed = Animator.StringToHash("Speed");
    readonly int _hashJump = Animator.StringToHash("Jump");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _firstScale = transform.localScale;
    }

    private void Update()
    {
        _animator.SetFloat(_hashSpeed, Mathf.Abs(_rigidbody.linearVelocityX));
        //移動する向きによってキャラクターを反転させる
        if (_rigidbody.linearVelocityX > 0)
        {
            transform.localScale = new Vector3(-_firstScale.x, _firstScale.y, _firstScale.z);
        }
        else if (_rigidbody.linearVelocityX < 0)
        {
            transform.localScale = _firstScale;
        }
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
