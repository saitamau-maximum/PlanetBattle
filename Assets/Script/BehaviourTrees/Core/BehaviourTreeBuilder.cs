using BlackboardSystem;
using UnityEngine;
namespace BehaviourTrees
{
    /// <summary>
    /// ビヘイビアツリーの構築を行うクラスの基底クラス。
    /// </summary>
    public abstract class BehaviourTreeBuilder : ScriptableObject
    {
        public abstract BehaviourTree Build(Blackboard bb);
    }
}