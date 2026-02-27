using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class ChangeDirection : Node
    {
        public override Status Process()
        {
            Transform self = _blackboard.GetValue(CharacterKeys.SelfTransform);
            self.rotation *= Quaternion.Euler(0, 180, 0);
            return Status.Success;
        }

        public override void Reset() { }
    }
}