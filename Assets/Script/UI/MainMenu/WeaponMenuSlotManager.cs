using System.Collections.Generic;
using UnityEngine;

public class WeaponMenuSlotManager : MenuSlotManager<PlayerWeaponData>
{
    [SerializeField] private GameSaveData _gameData;
    [SerializeField] private WeaponDatabase _weaponDatabase;

    protected override IReadOnlyList<PlayerWeaponData> SelectedData => _gameData.SelectedWeapons;
    protected override IReadOnlyList<PlayerWeaponData> InventoryData => _weaponDatabase.Database;

    protected override void SaveSelection(int index, PlayerWeaponData data)
    {
        _gameData.SetSelectedWeapon(index, data);
    }
}
