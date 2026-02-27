using System;
using BlackboardSystem;

namespace BehaviourTrees
{
    //<summary>
    // RepeatWhile ノード。
    // 登録された条件式（Func<bool>）を評価し、
    // true の場合は子ノードを繰り返し実行し、false の場合は Success を返す。
    //</summary>
    public class RepeatWhile : DecoratorNode
    {
        private readonly Func<bool> _condition;

        public RepeatWhile(string name, Func<bool> condition, Node child)
            : base(name, child)
        {
            _condition = condition;
        }

        public override Status Process()
        {
            if (!_condition())
            {
                Reset();
                return Status.Success;
            }

            _child.Process();
            return Status.Running;
        }
    }
}