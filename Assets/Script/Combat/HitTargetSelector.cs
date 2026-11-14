using System.Collections.Generic;
using UnityEngine;

public abstract class HitTargetSelector : MonoBehaviour
{
    public abstract Health SelectTarget(IReadOnlyList<Health> candidates);
}
