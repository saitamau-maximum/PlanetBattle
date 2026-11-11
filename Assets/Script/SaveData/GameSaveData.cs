using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Save/GameSaveData")]
public class GameSaveData : ScriptableObject
{
    public const int WEAPON_SLOT_COUNT = 2;
    public const int STRUCTURE_SLOT_COUNT = 4;

    [Header("Weapons")]
    [SerializeField] private List<PlayerWeaponData> _selectedWeapons = new();

    [Header("Structures")]
    [SerializeField] private List<StructureData> _selectedStructures = new();

    public IReadOnlyList<PlayerWeaponData> SelectedWeapons
    {
        get
        {
            ValidateWeaponSlotCount(_selectedWeapons.Count);
            return _selectedWeapons;
        }
    }

    public IReadOnlyList<StructureData> SelectedStructures
    {
        get
        {
            ValidateStructureSlotCount(_selectedStructures.Count);
            return _selectedStructures;
        }
    }

    private void OnValidate()
    {
        if (_selectedWeapons.Count != WEAPON_SLOT_COUNT)
        {
            Debug.LogError($"SelectedWeapons must contain exactly {WEAPON_SLOT_COUNT} weapons (current: {_selectedWeapons.Count}).", this);
        }

        if (_selectedStructures.Count != STRUCTURE_SLOT_COUNT)
        {
            Debug.LogError($"SelectedStructures must contain exactly {STRUCTURE_SLOT_COUNT} structures (current: {_selectedStructures.Count}).", this);
        }
    }

    private static void ValidateWeaponSlotCount(int count)
    {
        if (count != WEAPON_SLOT_COUNT)
        {
            throw new InvalidOperationException($"SelectedWeapons must contain exactly {WEAPON_SLOT_COUNT} weapons (current: {count}).");
        }
    }

    private static void ValidateStructureSlotCount(int count)
    {
        if (count != STRUCTURE_SLOT_COUNT)
        {
            throw new InvalidOperationException($"SelectedStructures must contain exactly {STRUCTURE_SLOT_COUNT} structures (current: {count}).");
        }
    }

    public void SetSelectedWeapons(IEnumerable<PlayerWeaponData> weapons)
    {
        List<PlayerWeaponData> selectedWeapons = new(weapons);
        ValidateWeaponSlotCount(selectedWeapons.Count);

        _selectedWeapons.Clear();
        _selectedWeapons.AddRange(selectedWeapons);
    }

    public void SetSelectedWeapon(int index, PlayerWeaponData weapon)
    {
        if (index < 0 || index >= _selectedWeapons.Count)
        {
            return;
        }

        _selectedWeapons[index] = weapon;
    }

    public void SetSelectedStructures(IEnumerable<StructureData> structures)
    {
        List<StructureData> selectedStructures = new(structures);
        ValidateStructureSlotCount(selectedStructures.Count);

        _selectedStructures.Clear();
        _selectedStructures.AddRange(selectedStructures);
    }

    public void SetSelectedStructure(int index, StructureData structure)
    {
        if (index < 0 || index >= _selectedStructures.Count)
        {
            return;
        }

        _selectedStructures[index] = structure;
    }
}
