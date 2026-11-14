using System.Collections.Generic;

public class StructurePriorityTargetSelector : HitTargetSelector
{
    public override Health SelectTarget(IReadOnlyList<Health> candidates)
    {
        foreach (Health candidate in candidates)
        {
            if (candidate.TryGetComponent<Structure>(out _)) return candidate;
        }

        return candidates.Count > 0 ? candidates[0] : null;
    }
}
