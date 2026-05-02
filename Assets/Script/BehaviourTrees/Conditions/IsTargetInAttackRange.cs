using System;
using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class IsTargetInAttackRange : Condition
    {
        public IsTargetInAttackRange() : base("IsTargetInAttackRange") { }

        protected override bool Check()
        {
            Transform target = _blackboard.GetValue(CharacterKeys.TargetTransform);
            if (target == null) target = _blackboard.GetValue(CharacterKeys.BaseTargetTransform);
            Transform self = _blackboard.GetValue(CharacterKeys.SelfTransform);
            Vector3 offsetToTarget = target.position - self.position;
            float attackRange = _blackboard.GetValue(CharacterKeys.CharacterContext).AttackRange;
            const float STEP_HEIGHT = 0.4f;

            return Math.Abs(offsetToTarget.x) < attackRange && Mathf.Abs(offsetToTarget.y) < STEP_HEIGHT;
        }
    }
}