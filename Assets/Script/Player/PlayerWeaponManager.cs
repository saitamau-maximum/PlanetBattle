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
            weaponInstance.Unequip();
            _weapons.Add(weaponInstance);
        }
    }

    public bool TryUseWeapon(int index)
    {
        if (IsAttaking() || _weapons[_currentWeaponIndex].IsCoolingDown()) return false;

        if (_currentWeaponIndex != index)
            _weapons[_currentWeaponIndex].Unequip();

        _currentWeaponIndex = index;

        _weapons[_currentWeaponIndex].Equip();
        return _weapons[_currentWeaponIndex].TryAttack();
    }

    public void UnequipCurrentWeapon()
    {
        _weapons[_currentWeaponIndex].Unequip();
    }

    public bool IsAttaking()
    {
        return _weapons[_currentWeaponIndex].IsAttacking();
    }

    public string GetWeaponName(int index)
    {
        return _weapons[index].GetWeaponName();
    }
}
