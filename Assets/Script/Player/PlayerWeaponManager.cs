using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponBase[] _weapons;
    private int _currentWeaponIndex = 0;

    public const int PRIMARY_WEAPON_INDEX = 0;
    public const int SECONDARY_WEAPON_INDEX = 1;

    public WeaponBase.WeaponState CurrentWeaponState => _weapons[_currentWeaponIndex].CurrentState;
    public string GetCurrentWeaponName => _weapons[_currentWeaponIndex].WeaponName;

    private void Start()
    {
        foreach (var weapon in _weapons)
        {
            weapon.Unequip();
        }
    }

    public bool TryUsePrimaryWeapon()
    {
        return TryUseWeapon(0);
    }
    public bool TryUseSecondaryWeapon()
    {
        return TryUseWeapon(1);
    }

    private bool TryUseWeapon(int index)
    {
        if (CurrentWeaponState != WeaponBase.WeaponState.Idle) return false;

        if (_currentWeaponIndex != index)
            _weapons[_currentWeaponIndex].Unequip();

        _currentWeaponIndex = index;

        _weapons[_currentWeaponIndex].Equip();
        return _weapons[_currentWeaponIndex].TryUseWeapon();
    }

    public void UnequipCurrentWeapon()
    {
        _weapons[_currentWeaponIndex].Unequip();
    }
}
