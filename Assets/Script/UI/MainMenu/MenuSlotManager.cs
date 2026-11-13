using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuSlotManager<TData> : MonoBehaviour
    where TData : ScriptableObject
{
    [SerializeField] private Transform _loadoutContent;
    [SerializeField] private Transform _inventoryContent;
    [SerializeField] private MenuSlotView<TData> _slotPrefab;

    // インベントリで選んだデータの入れ替え先となる、選択中の持ち込みスロット。
    private MenuSlotView<TData> _targetSlot;
    private bool _selectDefaultTarget = true;

    private readonly List<MenuSlotView<TData>> _loadoutSlots = new();
    private readonly List<MenuSlotView<TData>> _inventorySlots = new();

    public event Action OnTargetSelected;

    protected abstract IReadOnlyList<TData> SelectedData { get; }
    protected abstract IReadOnlyList<TData> InventoryData { get; }

    protected abstract void SaveSelection(int index, TData data);

    private void Start()
    {
        CreateLoadoutSlots();
        if (_selectDefaultTarget)
        {
            InitializeTargetSlot();
        }
        CreateInventorySlots();
        UpdateInventoryFrames();
    }

    private void OnDestroy()
    {
        foreach (var slot in _loadoutSlots)
        {
            slot.OnClicked -= SelectTargetSlot;
        }

        foreach (var slot in _inventorySlots)
        {
            slot.OnClicked -= SelectData;
        }
    }

    public void ClearTargetSlot()
    {
        _targetSlot = null;
        // Start前に解除されても、初期ターゲットを自動選択しない。
        _selectDefaultTarget = false;

        foreach (var slot in _loadoutSlots)
        {
            slot.HideFrame();
        }
    }

    private void CreateLoadoutSlots()
    {
        foreach (var data in SelectedData)
        {
            MenuSlotView<TData> slot = Instantiate(
                _slotPrefab,
                _loadoutContent
            );

            slot.SetData(data);
            slot.OnClicked += SelectTargetSlot;

            _loadoutSlots.Add(slot);
        }
    }

    private void InitializeTargetSlot()
    {
        if (_loadoutSlots.Count == 0)
        {
            return;
        }

        _targetSlot = _loadoutSlots[^1];
        _targetSlot.ShowFrame();
    }

    private void CreateInventorySlots()
    {
        foreach (var data in InventoryData)
        {
            MenuSlotView<TData> slot = Instantiate(
                _slotPrefab,
                _inventoryContent
            );

            slot.SetData(data);
            slot.OnClicked += SelectData;

            _inventorySlots.Add(slot);
        }
    }

    private void SelectTargetSlot(MenuSlotView<TData> selectedSlot)
    {
        _targetSlot = selectedSlot;

        foreach (var slot in _loadoutSlots)
        {
            slot.HideFrame();
        }

        _targetSlot.ShowFrame();
        OnTargetSelected?.Invoke();
    }

    private void SelectData(MenuSlotView<TData> selectedSlot)
    {
        if (_targetSlot == null)
        {
            return;
        }

        // 既に選択されているデータなら何もしない
        if (IsSelected(selectedSlot.Data))
        {
            return;
        }

        int targetIndex = _loadoutSlots.IndexOf(_targetSlot);

        if (targetIndex < 0)
        {
            throw new InvalidOperationException("The target slot does not belong to the loadout slots.");
        }

        SaveSelection(targetIndex, selectedSlot.Data);
        _targetSlot.SetData(selectedSlot.Data);

        // インベントリ側の選択状態を更新
        UpdateInventoryFrames();
    }

    private bool IsSelected(TData data)
    {
        foreach (var slot in _loadoutSlots)
        {
            if (slot.Data == data)
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateInventoryFrames()
    {
        foreach (var inventorySlot in _inventorySlots)
        {
            if (IsSelected(inventorySlot.Data))
            {
                inventorySlot.ShowFrame();
            }
            else
            {
                inventorySlot.HideFrame();
            }
        }
    }
}
