using UnityEngine;

[CreateAssetMenu(menuName = "StructureBuilding/StructureData")]
public class StructureData : ScriptableObject
{
    public GameObject Prefab;
    public int Cost;
    public Vector2 GridSize;
}
