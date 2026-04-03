using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class Jump : Node
    {
        public override Status Process()
        {
            Rigidbody2D rb = _blackboard.GetValue(CharacterKeys.SelfRigidbody2D);
            float jumpForce = _blackboard.GetValue(CharacterKeys.CharacterContext).JumpForce;

            rb.linearVelocityY = jumpForce;
            return Status.Success;
        }

        public override void Reset() { }
    }
}