using System;

namespace BehaviourTrees
{
    /// <summary>
    /// Condition ノード。
    /// 登録された条件式（Func<bool>）を評価し、
    /// true の場合は Success、false の場合は Failure を返す。
    /// 状態を持たず、判定のみを行う葉ノード。
    /// </summary>

    public class Condition : IBehaviour
    {
        readonly Func<bool> _condition;

        public Condition(Func<bool> condition)
        {
            _condition = condition;
        }

        public Node.Status Process()
        {
            return _condition() ? Node.Status.Success : Node.Status.Failure;
        }
    }
}