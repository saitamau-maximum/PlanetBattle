using BlackboardSystem;
using UnityEngine;

namespace BehaviourTrees
{
    public class UseWeapon : Node
    {
        private readonly WeaponBase _weapon;
        public UseWeapon(WeaponBase weapon) : base("UseWeapon")
        {
            _weapon = weapon;
        }

        public override Status Process()
        {
            _weapon.TryAttack();

            if (_weapon.IsAttacking()) return Status.Running;

            return Status.Success;
        }

        public override void Reset() { }
    }
}