public class WeaponMenuSlot : MenuSlotView<PlayerWeaponData>
{
    protected override void UpdateView(PlayerWeaponData data)
    {
        SetImage(data.Image);
    }
}
