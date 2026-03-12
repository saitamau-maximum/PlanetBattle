using System;
using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class ShouldChangeDirection : Condition
    {
        public ShouldChangeDirection() : base("ShouldChangeDirection") { }

        protected override bool Check()
        {
            EnvironmentSensor sensor = _blackboard.GetValue(CharacterKeys.SelfEnvironmentSensor);
            Transform target = _blackboard.GetValue(CharacterKeys.TargetTransform);
            Transform self = _blackboard.GetValue(CharacterKeys.SelfTransform);

            const float STEP_HEIGHT = 0.4f;
            return (Mathf.Sign((target.position - self.position).x) != Mathf.Sign(self.right.x) && Mathf.Abs((target.position - self.position).y) < STEP_HEIGHT)
                || (!sensor.HasGroundFront() && !sensor.HasLandingSpotBelow());
        }
    }
}