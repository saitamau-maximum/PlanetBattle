using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] private List<WeaponBase> _weaponPrefabs;
    private readonly List<WeaponBase> _weapons = new();
    private int _currentWeaponIndex = 0;

    public const int PRIMARY_WEAPON_INDEX = 0;
    public const int SECONDARY_WEAPON_INDEX = 1;

    private void Awake()
    {
        foreach (var weaponPrefab in _weaponPrefabs)
        {
            WeaponBase weaponInstance = Instantiate(weaponPrefab, transform);
            weaponInstance.OnUnequip();
            _weapons.Add(weaponInstance);
        }
    }

    public bool TryUseWeapon(int index)
    {
        if (IsUsingWeapon()) return false;

        if (_currentWeaponIndex != index)
            _weapons[_currentWeaponIndex].OnUnequip();

        _currentWeaponIndex = index;

        _weapons[_currentWeaponIndex].OnEquip();
        return _weapons[_currentWeaponIndex].TryAttack();
    }

    public void UnequipCurrentWeapon()
    {
        _weapons[_currentWeaponIndex].OnUnequip();
    }

    public bool IsUsingWeapon()
    {
        return _weapons[_currentWeaponIndex].IsCoolingDown();
    }

    public string GetWeaponName(int index)
    {
        return _weapons[index].GetWeaponName();
    }
}
