using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Melee : WeaponBase
{
    private Animator _animator;
    readonly int _hashAttack = Animator.StringToHash("Attack");

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }
    protected override void Attack()
    {
        _animator.SetTrigger(_hashAttack);
    }
}