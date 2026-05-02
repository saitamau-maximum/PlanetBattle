using System;
using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class ShouldStepUp : Condition
    {
        public ShouldStepUp() : base("ShouldStepUp") { }

        protected override bool Check()
        {
            EnvironmentSensor sensor = _blackboard.GetValue(CharacterKeys.SelfEnvironmentSensor);
            Transform target = _blackboard.GetValue(CharacterKeys.TargetTransform);
            if (target == null) target = _blackboard.GetValue(CharacterKeys.BaseTargetTransform);
            Transform self = _blackboard.GetValue(CharacterKeys.SelfTransform);

            const float STEP_HEIGHT = 0.4f;
            return sensor.HasStepUpObstacle() && ((target.position - self.position).y > STEP_HEIGHT || !sensor.HasGroundFront());
        }
    }
}