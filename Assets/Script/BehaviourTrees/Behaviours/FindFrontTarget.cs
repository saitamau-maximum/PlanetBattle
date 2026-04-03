using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class FindFrontTarget : Node
    {
        public override Status Process()
        {
            Transform self = _blackboard.GetValue(CharacterKeys.SelfTransform);
            CharacterContext context = _blackboard.GetValue(CharacterKeys.CharacterContext);
            float serchRange = context.SearchRange;
            string targetTag = context.TargetTag;

            Collider2D[] hits = Physics2D.OverlapCircleAll(
                self.position,
                serchRange
            );

            if (hits.Length <= 0) return Status.Failure;

            Transform nearestTarget = null;
            float minDist = float.MaxValue;
            foreach (Collider2D obj in hits)
            {
                float dist = Vector2.Distance(self.position, obj.transform.position);

                if (dist < minDist && obj.tag == targetTag && Mathf.Sign((obj.transform.position - self.position).x) == Mathf.Sign(self.right.x))
                {
                    minDist = dist;
                    nearestTarget = obj.transform;
                }
            }

            if (nearestTarget == null) return Status.Failure;

            _blackboard.SetValue(CharacterKeys.TargetTransform, nearestTarget);
            return Status.Success;
        }

        public override void Reset() { }
    }
}