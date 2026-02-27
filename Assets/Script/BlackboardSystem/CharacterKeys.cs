using UnityEngine;

namespace BlackboardSystem
{
    public static class CharacterKeys
    {
        public static readonly BlackboardKey<Transform> SelfTransform = new("SelfTransform");
        public static readonly BlackboardKey<Rigidbody2D> SelfRigidbody2D = new("SelfRigidbody2D");
        public static readonly BlackboardKey<EnvironmentSensor> SelfEnvironmentSensor = new("SelfEnvironmentSensor");
        public static readonly BlackboardKey<Transform> TargetTransform = new("TargetTransform");
        public static readonly BlackboardKey<float> JumpForce = new("JumpForce");
        public static readonly BlackboardKey<float> AttackRange = new("AttackRange");

    }
}