using System;
using BlackboardSystem;

namespace BehaviourTrees
{
    //<summary>
    // RepeatForever ノード。
    // 子ノードをずっと繰り返し実行する。
    //</summary>
    public class RepeatForever : DecoratorNode
    {
        public RepeatForever(Node child) : base("RepeatForever", child) { }

        public override Status Process()
        {
            _child.Process();
            return Status.Running;
        }
    }
}