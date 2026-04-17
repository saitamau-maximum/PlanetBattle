using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ContactWeapon : WeaponBase
{
    private Animator _animator;
    private Hitbox[] _hitboxes;
    readonly int _hashAttack = Animator.StringToHash("Attack");

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _hitboxes = GetComponentsInChildren<Hitbox>(true);
    }

    private void Start()
    {
        foreach (var hitbox in _hitboxes)
        {
            hitbox.OnFirstHit += HandleFirstHit;
            hitbox.OnContinuousHit += HandleContinuousHit;
        }
    }

    private void OnDestroy()
    {
        foreach (var hitbox in _hitboxes)
        {
            hitbox.OnFirstHit -= HandleFirstHit;
            hitbox.OnContinuousHit -= HandleContinuousHit;
        }
    }

    private void HandleFirstHit(Health targetHealth)
    {
        targetHealth.TakeDamage(_weaponData.DamageAmount);
    }

    private void HandleContinuousHit(Health targetHealth)
    {
        //TODO:特殊効果付与
    }
    protected override IEnumerator AttackCoroutine()
    {
        _animator.SetTrigger(_hashAttack);

        int layerIndex = 0;
        float attackDuration = _animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        yield return new WaitForSeconds(attackDuration);
    }
}