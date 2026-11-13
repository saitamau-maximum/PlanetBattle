using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponDatabase")]
public class WeaponDatabase : ScriptableObject
{
    public List<PlayerWeaponData> Database = new();
}
