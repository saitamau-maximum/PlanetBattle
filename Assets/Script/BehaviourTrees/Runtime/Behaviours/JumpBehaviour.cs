using UnityEngine;

namespace BehaviourTrees
{
    public class JumpBehaviour : IBehaviour
    {
        private readonly Rigidbody2D _rb;
        private readonly float _jumpForce;

        public JumpBehaviour(float jumpForce, Rigidbody2D rb)
        {
            _jumpForce = jumpForce;
            _rb = rb;
        }

        public Node.Status Process()
        {
            _rb.linearVelocityY = _jumpForce;
            return Node.Status.Success;
        }
    }
}