using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class MoveForward : Node
    {
        private readonly float _speed;
        public MoveForward(float speed) : base("MoveForward")
        {
            _speed = speed;
        }

        public override Status Process()
        {
            Rigidbody2D rb = _blackboard.GetValue(CharacterKeys.SelfRigidbody2D);

            rb.linearVelocityX = rb.transform.right.x * _speed;

            //ジャンプ中は前に進み続ける
            if (Mathf.Abs(rb.linearVelocityY) < 0.01f)
                return Status.Success;
            else
                return Status.Running;
        }

        public override void Reset() { }
    }
}