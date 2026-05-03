using System.Collections.Generic;
using UnityEngine;

public class BuildingModeUI : MonoBehaviour
{
    [SerializeField] private StructureEntryUI entryPrefab;
    [SerializeField] private PlayerBuildingManager buildingManager;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _structureSlot;

    private List<StructureEntryUI> _entryUIList = new();

    private void Start()
    {
        for (int i = 0; i < buildingManager.Entries.Count; i++)
        {
            StructureEntryUI entryUI = Instantiate(entryPrefab, _structureSlot.transform);
            _entryUIList.Add(entryUI);
            string keyName = (i + 1).ToString();
            entryUI.Init(buildingManager.Entries[i], keyName);
        }

        buildingManager.OnSelectedStructureChanged += UpdateSelectedStructure;
        _playerController.OnModeChanged += SetActiveByMode;

        UpdateSelectedStructure(0);
        SetActiveByMode(_playerController.CurrentMode);
    }

    private void OnDestroy()
    {
        buildingManager.OnSelectedStructureChanged -= UpdateSelectedStructure;
        _playerController.OnModeChanged -= SetActiveByMode;
    }

    private void SetActiveByMode(PlayerController.Mode mode)
    {
        if (mode == PlayerController.Mode.Building)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateSelectedStructure(int index)
    {
        for (int i = 0; i < _entryUIList.Count; i++)
        {
            _entryUIList[i].SetSelected(index == i);
        }
    }
}
