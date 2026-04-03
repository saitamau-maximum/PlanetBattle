using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimationWeapon : WeaponBase
{
    private Animator _animator;
    private SpriteRenderer _renderer;
    readonly int _hashAttack = Animator.StringToHash("Attack");

    protected override void Awake()
    {
        base.Awake();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    protected override IEnumerator AttackCoroutine()
    {
        _animator.SetTrigger(_hashAttack);

        int layerIndex = 0;
        float attackDuration = _animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        yield return new WaitForSeconds(attackDuration);
    }

    public override void Equip()
    {
        _renderer.enabled = true;
    }

    public override void Unequip()
    {
        _renderer.enabled = false;
    }
}