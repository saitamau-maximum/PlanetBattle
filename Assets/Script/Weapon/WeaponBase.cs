using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected WeaponData _weaponData;
    protected bool _isAttacking = false;
    protected bool _isCoolingDown = false;

    protected virtual void Awake()
    {
        transform.localPosition = _weaponData.HoldOffset;
    }

    public bool TryAttack()
    {
        if (_isAttacking || _isCoolingDown) return false;

        StartCoroutine(AttackAndCooldown());

        return true;
    }

    private IEnumerator AttackAndCooldown()
    {
        _isAttacking = true;
        yield return StartCoroutine(AttackCoroutine());
        _isAttacking = false;

        _isCoolingDown = true;
        yield return new WaitForSeconds(_weaponData.CoolTime);
        _isCoolingDown = false;
    }

    protected abstract IEnumerator AttackCoroutine();
    public abstract void Equip();
    public abstract void Unequip();

    public bool IsAttacking() => _isAttacking;
    public bool IsCoolingDown() => _isCoolingDown;
    public string GetWeaponName() => _weaponData.WeaponName;
}