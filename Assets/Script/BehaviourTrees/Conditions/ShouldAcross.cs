using System;
using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class ShouldAcross : Condition
    {
        public ShouldAcross() : base("ShouldAcross") { }

        protected override bool Check()
        {
            EnvironmentSensor sensor = _blackboard.GetValue(CharacterKeys.SelfEnvironmentSensor);
            Transform target = _blackboard.GetValue(CharacterKeys.TargetTransform);
            if (target == null) target = _blackboard.GetValue(CharacterKeys.BaseTargetTransform);
            Transform self = _blackboard.GetValue(CharacterKeys.SelfTransform);

            return !sensor.HasGroundFront() && sensor.HasGroundAcrossGap() && (target.position - self.position).y >= 0;
        }
    }
}