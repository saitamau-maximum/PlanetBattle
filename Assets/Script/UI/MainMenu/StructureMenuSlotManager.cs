using System.Collections.Generic;
using UnityEngine;

public class StructureMenuSlotManager : MenuSlotManager<StructureData>
{
    [SerializeField] private GameSaveData _gameData;
    [SerializeField] private StructureDatabase _structureDatabase;

    protected override IReadOnlyList<StructureData> SelectedData => _gameData.SelectedStructures;
    protected override IReadOnlyList<StructureData> InventoryData => _structureDatabase.Database;

    protected override void SaveSelection(int index, StructureData data)
    {
        _gameData.SetSelectedStructure(index, data);
    }
}
