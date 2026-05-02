using System;
using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class IsTargetInRange : Condition
    {
        private readonly Transform _target;
        private readonly float _range;
        public IsTargetInRange(Transform target, float range) : base("IsTargetInRange")
        {
            _target = target == null ? _blackboard.GetValue(CharacterKeys.BaseTargetTransform) : target;
            _range = range;
        }

        protected override bool Check()
        {
            Transform self = _blackboard.GetValue(CharacterKeys.SelfTransform);
            Vector3 offsetToTarget = _target.position - self.position;
            const float STEP_HEIGHT = 0.4f;

            return Math.Abs(offsetToTarget.x) < _range && Mathf.Abs(offsetToTarget.y) < STEP_HEIGHT;
        }
    }
}