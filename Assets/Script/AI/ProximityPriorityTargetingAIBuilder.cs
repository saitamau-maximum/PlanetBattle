using BehaviourTrees;
using BlackboardSystem;
using UnityEngine;

//<summary>
// サーチ範囲に敵がいる場合、ターゲットを変更する。
// BaseTargetとの距離が設定より小さい場合、ターゲットの変更は起きない
//</summary>
[CreateAssetMenu(menuName = "AI/ProximityPriorityTargetingAIBuilder")]
public class ProximityPriorityTargetingAIBuilder : NormalAIBuilder
{
    public override BehaviourTree Build(Blackboard bb)
    {
        Transform baseTarget = bb.GetValue<Transform>(CharacterKeys.BaseTargetTransform);
        float baseTargetLockRange = bb.GetValue<CharacterContext>(CharacterKeys.CharacterContext).BaseTargetLockRange;

        BehaviourTree behaviourTree = new BehaviourTree();
        behaviourTree.AddChild(new RepeatForever(
            new Sequence("rootSequence",
                new Selector("SetTargetSelector",
                    new Sequence("FindNearestTargetSequence",
                        new Inverter("TargetIsNotInRange", new IsTargetInRange(baseTarget, baseTargetLockRange)),
                        new FindNearestTarget()
                    ),
                    new Action(() => { bb.SetValue(CharacterKeys.TargetTransform, baseTarget); })
                ),
                new Selector("root",
                    CreateAttackNode(bb),
                    CreateGoTotargetNode(bb)
                )
            )
        ));
        return behaviourTree;
    }
}