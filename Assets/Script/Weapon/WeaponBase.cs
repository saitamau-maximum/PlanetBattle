using System.Collections;
using UnityEngine;

/*
ダメージを与える
特殊効果付与
クールタイム管理
*/
public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected WeaponData _weaponData;
    protected Hitbox[] _hitboxes;

    public enum WeaponState
    {
        Idle,
        Attacking,
        CoolingDown
    }

    public WeaponState CurrentState { get; private set; }

    public string WeaponName => _weaponData.WeaponName;

    protected virtual void Awake()
    {
        _hitboxes = GetComponentsInChildren<Hitbox>(true);
    }

    protected virtual void Start()
    {
        foreach (var hitbox in _hitboxes)
        {
            hitbox.OnFirstHit += HandleFirstHit;
            hitbox.OnContinuousHit += HandleContinuousHit;
        }
    }

    protected virtual void OnDestroy()
    {
        foreach (var hitbox in _hitboxes)
        {
            hitbox.OnFirstHit -= HandleFirstHit;
            hitbox.OnContinuousHit -= HandleContinuousHit;
        }
    }

    protected virtual void HandleFirstHit(Health targetHealth)
    {
        targetHealth.TakeDamage(_weaponData.DamageAmount);
    }

    protected virtual void HandleContinuousHit(Health targetHealth)
    {
        //TODO:特殊効果付与
    }

    public bool TryUseWeapon()
    {
        if (CurrentState != WeaponState.Idle) return false;

        StartCoroutine(AttackAndCooldown());

        return true;
    }

    private IEnumerator AttackAndCooldown()
    {
        CurrentState = WeaponState.Attacking;
        yield return StartCoroutine(AttackCoroutine());

        CurrentState = WeaponState.CoolingDown;
        yield return new WaitForSeconds(_weaponData.CoolTime);

        CurrentState = WeaponState.Idle;
    }

    protected abstract IEnumerator AttackCoroutine();

    public abstract void Equip();
    public abstract void Unequip();
}