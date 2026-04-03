using UnityEngine;

namespace BlackboardSystem
{
    public static class CharacterKeys
    {
        public static readonly BlackboardKey<Transform> SelfTransform = new("SelfTransform");
        public static readonly BlackboardKey<Rigidbody2D> SelfRigidbody2D = new("SelfRigidbody2D");
        public static readonly BlackboardKey<EnvironmentSensor> SelfEnvironmentSensor = new("SelfEnvironmentSensor");
        public static readonly BlackboardKey<Transform> TargetTransform = new("TargetTransform");
        public static readonly BlackboardKey<Transform> BaseTargetTransform = new("BaseTargetTransform");
        public static readonly BlackboardKey<CharacterContext> CharacterContext = new("CharacterContext");
        public static readonly BlackboardKey<WeaponBase> Weapon = new("Weapon");
    }
}