using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class ChangeDirectionToTarget : Node
    {
        public override Status Process()
        {
            Transform target = _blackboard.GetValue(CharacterKeys.TargetTransform);
            Transform self = _blackboard.GetValue(CharacterKeys.SelfTransform);

            self.right = new Vector3((target.position - self.position).normalized.x, 0, 0);
            return Status.Success;
        }

        public override void Reset() { }
    }
}