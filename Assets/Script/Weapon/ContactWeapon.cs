using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ContactWeapon : WeaponBase
{
    [SerializeField] private string _attackAnimationClipName = "Attack";
    private Animator _animator;
    private Hitbox[] _hitboxes;
    private float _attackDuration;
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

        int animationHash = Animator.StringToHash(_attackAnimationClipName);
        foreach (var clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (Animator.StringToHash(clip.name) == animationHash) _attackDuration = clip.length;
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

        yield return new WaitForSeconds(_attackDuration);
    }
}