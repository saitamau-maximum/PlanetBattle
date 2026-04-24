using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drop/DropTable")]
public class DropTable : ScriptableObject
{
    public DropEntry[] Entries;
}

[Serializable]
public class DropEntry
{
    public GameObject Prefab;
    public float Probability;
    public int MinCount;
    public int MaxCount;
}