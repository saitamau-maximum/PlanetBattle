using UnityEngine;

[CreateAssetMenu(menuName = "StructureBuilding/StructureData")]
public class StructureData : ScriptableObject
{
    public GameObject Prefab;
    public Sprite Image;
    public int Cost;
    public float BuildTime;
    public Vector2 GridSize;
}
