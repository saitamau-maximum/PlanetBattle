using System;
using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class IsInAir : Condition
    {
        public IsInAir() : base("IsInAir") { }

        protected override bool Check()
        {
            Rigidbody2D rigidbody = _blackboard.GetValue(CharacterKeys.SelfRigidbody2D);

            const float VELOCITY_Y_EPSILON = 0.01f;
            return Math.Abs(rigidbody.linearVelocityY) > VELOCITY_Y_EPSILON;
        }
    }
}