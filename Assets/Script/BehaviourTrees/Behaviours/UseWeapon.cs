using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class UseWeapon : Node
    {
        public override Status Process()
        {
            WeaponBase weapon = _blackboard.GetValue(CharacterKeys.Weapon);
            weapon.TryAttack();

            if (weapon.IsAttacking()) return Status.Running;

            return Status.Success;
        }

        public override void Reset() { }
    }
}