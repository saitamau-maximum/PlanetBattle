using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ProjectileWeapon : WeaponBase
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _muzzle;
    private Animator _animator;
    readonly int _hashAttack = Animator.StringToHash("Attack");

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }
    protected override IEnumerator AttackCoroutine()
    {
        Projectile projectile = Instantiate(_projectilePrefab, _muzzle.position, _muzzle.rotation);
        projectile.Init(_weaponData.DamageAmount);

        _animator.SetTrigger(_hashAttack);
        int layerIndex = 0;
        float attackDuration = _animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        yield return new WaitForSeconds(attackDuration);
    }
}