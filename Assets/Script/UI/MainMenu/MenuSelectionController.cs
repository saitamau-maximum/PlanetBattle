using UnityEngine;

public class MenuSelectionController : MonoBehaviour
{
    private enum SelectionType
    {
        Structure,
        Weapon
    }

    [SerializeField] private GameObject _structureInventory;
    [SerializeField] private GameObject _weaponInventory;
    [SerializeField] private StructureMenuSlotManager _structureSlotManager;
    [SerializeField] private WeaponMenuSlotManager _weaponSlotManager;
    [SerializeField] private SelectionType _initialSelection = SelectionType.Weapon;

    private void OnEnable()
    {
        _structureSlotManager.OnTargetSelected += ShowStructureSelection;
        _weaponSlotManager.OnTargetSelected += ShowWeaponSelection;
    }

    private void OnDisable()
    {
        _structureSlotManager.OnTargetSelected -= ShowStructureSelection;
        _weaponSlotManager.OnTargetSelected -= ShowWeaponSelection;
    }

    private void Start()
    {
        ShowSelection(_initialSelection);
    }

    public void ShowStructureSelection()
    {
        ShowSelection(SelectionType.Structure);
    }

    public void ShowWeaponSelection()
    {
        ShowSelection(SelectionType.Weapon);
    }

    private void ShowSelection(SelectionType type)
    {
        if (type == SelectionType.Structure)
        {
            _weaponSlotManager.ClearTargetSlot();
        }
        else
        {
            _structureSlotManager.ClearTargetSlot();
        }

        _structureInventory.SetActive(type == SelectionType.Structure);
        _weaponInventory.SetActive(type == SelectionType.Weapon);
    }
}
