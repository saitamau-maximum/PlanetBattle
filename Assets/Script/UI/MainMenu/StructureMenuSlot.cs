using TMPro;
using UnityEngine;

public class StructureMenuSlot : MenuSlotView<StructureData>
{
    [SerializeField] private TextMeshProUGUI _costText;

    protected override void UpdateView(StructureData data)
    {
        SetImage(data.Image);
        _costText.text = data.Cost.ToString();
    }
}
