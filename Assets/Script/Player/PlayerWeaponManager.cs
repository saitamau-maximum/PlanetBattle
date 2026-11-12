using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] private GameSaveData _gameData;
    [SerializeField] private Transform _weaponParent;
    private WeaponBase[] _weapons;

    // -1は、現在武器が装備されていないことを意味します。
    private int _currentWeaponIndex = -1;

    public IReadOnlyList<PlayerWeaponData> SelectedWeapons => _gameData.SelectedWeapons;
    public event Action<int> OnSelectedWeaponChanged;

    private WeaponBase CurrentWeapon => _weapons[_currentWeaponIndex];

    public WeaponBase.WeaponState CurrentWeaponState =>
        _currentWeaponIndex == -1 ? WeaponBase.WeaponState.Idle : CurrentWeapon.CurrentState;

    public string GetCurrentWeaponName =>
        _currentWeaponIndex == -1 ? string.Empty : CurrentWeapon.WeaponName;

    private void Start()
    {
        InitWeapons();
        // デフォルトは0、攻撃が発生するまでは武器を隠しておく。
        SelectWeapon(0);
    }

    private void OnDestroy()
    {
        // Start前に破棄された場合は、武器がまだ生成されていない。
        if (_weapons == null) return;

        foreach (var weapon in _weapons)
        {
            weapon.OnAttackCompleted -= HandleWeaponAttackCompleted;
        }
    }

    private void InitWeapons()
    {
        IReadOnlyList<PlayerWeaponData> selectedWeapons = SelectedWeapons;
        _weapons = new WeaponBase[GameSaveData.WEAPON_SLOT_COUNT];
        for (int i = 0; i < GameSaveData.WEAPON_SLOT_COUNT; i++)
        {
            PlayerWeaponData data = selectedWeapons[i];
            WeaponBase weaponBase = Instantiate(data.weaponBase, _weaponParent);

            weaponBase.transform.localPosition = data.EquippedOffset;
            _weapons[i] = weaponBase;
            _weapons[i].OnAttackCompleted += HandleWeaponAttackCompleted;
            _weapons[i].Unequip();
        }
    }

    public void SelectWeapon(int index)
    {
        if (index < 0 || index >= _weapons.Length) return;
        if (CurrentWeaponState == WeaponBase.WeaponState.Attacking) return;

        UnequipCurrentWeapon();

        _currentWeaponIndex = index;
        OnSelectedWeaponChanged?.Invoke(_currentWeaponIndex);
    }

    public bool TryUseSelectedWeapon()
    {
        if (_currentWeaponIndex == -1 || CurrentWeaponState != WeaponBase.WeaponState.Idle) return false;

        CurrentWeapon.Equip();
        return CurrentWeapon.TryUseWeapon();
    }

    public void UnequipCurrentWeapon()
    {
        if (_currentWeaponIndex != -1)
            CurrentWeapon.Unequip();
    }

    private void HandleWeaponAttackCompleted(WeaponBase weapon)
    {
        weapon.Unequip();
    }
}
