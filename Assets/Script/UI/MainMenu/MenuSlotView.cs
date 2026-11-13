using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuSlotView<TData> : MonoBehaviour
{
    [SerializeField] private GameObject _frame;
    [SerializeField] private Image _image;

    public TData Data { get; private set; }
    public event Action<MenuSlotView<TData>> OnSelect;

    public void OnClick()
    {
        OnSelect?.Invoke(this);
    }

    public void SetData(TData data)
    {
        Data = data;
        UpdateView(data);
    }

    protected abstract void UpdateView(TData data);

    protected void SetImage(Sprite image)
    {
        _image.sprite = image;
    }

    public void ShowFrame()
    {
        _frame.SetActive(true);
    }

    public void HideFrame()
    {
        _frame.SetActive(false);
    }
}
