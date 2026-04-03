using BehaviourTrees;
using BlackboardSystem;
using UnityEngine;

//<summary>
// ターゲットを追跡し続ける。
// 射程距離圏内にターゲットがいると攻撃する。
//</summary>
[CreateAssetMenu(menuName = "AI/NormalAIBuilder")]
public class NormalAIBuilder : BehaviourTreeBuilder
{
    public override BehaviourTree Build(Blackboard bb)
    {
        BehaviourTree behaviourTree = new BehaviourTree();
        behaviourTree.AddChild(new RepeatForever(
            new Selector("root",
                CreateAttackNode(bb),
                CreateGoTotargetNode(bb)
            )
        ));
        return behaviourTree;
    }
    protected Node CreateGoTotargetNode(Blackboard bb)
    {
        var environmentSensor = bb.GetValue(CharacterKeys.SelfEnvironmentSensor);

        CharacterContext context = bb.GetValue(CharacterKeys.CharacterContext);
        float speed = context.Speed;
        float jumpSpeed = context.JumpForwardSpeed;

        return new Selector("goToTargetSelector",
            new Sequence("InAir",
                new IsInAir(),
                new StopVelocityX()
            ),
            new Sequence("changeDirectionSequence",
                new ShouldChangeDirection(),
                new ChangeDirection()
            ),
            new Sequence("jumpAcrossSequence",
                new ShouldAcross(),
                new Jump(),
                new MoveForward(jumpSpeed)
            ),
            new Sequence("stepDownSequence",
                new ShouldStepDown(),
                new RepeatWhile("", () => environmentSensor.HasGroundBehind(), new MoveForward(speed)),
                new StopVelocityX()
            ),
            new Sequence("stepUpSequence",
                new ShouldStepUp(),
                new Jump(),
                new MoveForward(jumpSpeed)
            ),
            new Sequence("moveForwardSequence",
                new MoveForward(speed)
            )
        );
    }
    protected virtual Node CreateAttackNode(Blackboard bb)
    {
        return new Sequence("attackSequence",
            new IsTargetInAttackRange(),
            new StopVelocityX(),
            new BehaviourTrees.WaitForSeconds(0.3f),
            new UseWeapon()
        //new ChangeDirectionToTarget()
        );
    }
}