using System.Collections.Generic;

namespace BehaviourTrees
{
    /// <summary>
    /// Selector ノード。
    /// 子ノードを上から順に評価し、
    /// 最初に Success または Running を返した子ノードの結果を採用する。
    /// すべての子ノードが Failure の場合のみ Failure を返す。
    /// （優先度付きの OR 判定を行う制御ノード）
    /// </summary>
    public class Selector : CompositeNode
    {
        public Selector(string name) : base(name) { }
        public Selector(string name, params Node[] children) : base(name)
        {
            _children = new List<Node>(children);
        }

        public override Status Process()
        {
            while (_currentChild < _children.Count)
            {
                switch (_children[_currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default:
                        _currentChild++;
                        return Status.Running;
                }
            }
            Reset();
            return Status.Failure;
        }
    }
}