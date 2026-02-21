using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class MoveForward : Node
    {
        public override Status Process()
        {
            Rigidbody2D rb = _blackboard.GetValue(CharacterKeys.SelfRigidbody2D);
            float speed = _blackboard.GetValue(CharacterKeys.Speed);


            rb.linearVelocityX = rb.transform.right.x * speed;

            //ジャンプ中は前に進み続ける
            if (Mathf.Abs(rb.linearVelocityY) < 0.01f)
                return Status.Success;
            else
                return Status.Running;
        }

        public override void Reset() { }
    }
}