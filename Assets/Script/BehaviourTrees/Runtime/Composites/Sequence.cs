using UnityEngine;

namespace BehaviourTrees
{
    /// <summary>
    /// Sequence ノード。
    /// 子ノードを上から順に実行し、
    /// すべての子ノードが Success を返した場合のみ Success を返す。
    /// 途中で Failure が返された時点で Failure を返し処理を中断する。
    /// （AND 条件を表す制御ノード）
    /// </summary>

    public class Sequence : CompositeNode
    {
        public Sequence(string name) : base(name) { }

        public override Status Process()
        {
            Debug.Log($"Processing Sequence: {Name}");

            if (_currentChild < _children.Count)
            {
                switch (_children[_currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    default:
                        _currentChild++;
                        return _currentChild == _children.Count ? Status.Success : Status.Running;
                }
            }

            Reset();
            return Status.Success;
        }
    }
}