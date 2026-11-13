using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponData", menuName = "Weapon/PlayerWeaponData")]
public class PlayerWeaponData : ScriptableObject
{
    public WeaponBase weaponBase;
    public Vector2 EquippedOffset;
    public Sprite Image;

    [Tooltip("攻撃中に再度攻撃ボタンを押したとき、急降下できるか")]
    public bool CanFastFall;
}
