using System.Collections.Generic;
using UnityEngine;

public class RandomHitTargetSelector : HitTargetSelector
{
    public override Health SelectTarget(IReadOnlyList<Health> candidates)
    {
        return candidates.Count > 0
            ? candidates[Random.Range(0, candidates.Count)]
            : null;
    }
}
