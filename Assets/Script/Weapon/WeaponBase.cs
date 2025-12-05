using UnityEngine;
using Utility;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected WeaponData _weaponData;
    private CountdownTimer coolTimer;

    protected virtual void Awake()
    {
        coolTimer = new CountdownTimer(_weaponData.CoolTime);
        transform.localPosition = _weaponData.HoldOffset;
    }
    private void Update()
    {
        coolTimer.Tick();
    }
    public void OnEquip()
    {
        gameObject.SetActive(true);
    }
    public void OnUnequip()
    {
        gameObject.SetActive(false);
    }
    public string GetWeaponName()
    {
        return _weaponData.WeaponName;
    }

    public bool TryAttack()
    {
        if (coolTimer.IsFinished())
        {
            Attack();
            coolTimer.Start();
            return true;
        }
        return false;
    }

    public bool IsCoolingDown()
    {
        return !coolTimer.IsFinished();
    }
    protected abstract void Attack();
}