using UnityEngine;

namespace BehaviourTrees
{
    public class MoveForwardBehaviour : IBehaviour
    {
        private readonly float _speed;
        private readonly Rigidbody2D _rb;
        public MoveForwardBehaviour(float speed, Rigidbody2D rb)
        {
            _speed = speed;
            _rb = rb;
        }

        public Node.Status Process()
        {
            _rb.linearVelocityX = _rb.transform.right.x * _speed;
            return Node.Status.Running;
        }

        public void Reset()
        {
            _rb.linearVelocityX = 0;
        }
    }
}