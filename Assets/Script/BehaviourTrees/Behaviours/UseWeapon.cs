using BlackboardSystem;

namespace BehaviourTrees
{
    public class UseWeapon : Node
    {
        public override Status Process()
        {
            WeaponBase weapon = _blackboard.GetValue(CharacterKeys.Weapon);
            weapon.TryUseWeapon();

            if (weapon.CurrentState == WeaponBase.WeaponState.Attacking) return Status.Running;

            return Status.Success;
        }

        public override void Reset() { }
    }
}