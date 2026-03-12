using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class StopVelocityX : Node
    {
        public override Status Process()
        {
            Rigidbody2D rb = _blackboard.GetValue(CharacterKeys.SelfRigidbody2D);
            rb.linearVelocityX = 0;
            return Status.Success;
        }

        public override void Reset() { }
    }
}