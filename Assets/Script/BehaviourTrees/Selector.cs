namespace BehaviourTrees
{
    /// <summary>
    /// Selector ノード。
    /// 子ノードを上から順に評価し、
    /// 最初に Success または Running を返した子ノードの結果を採用する。
    /// すべての子ノードが Failure の場合のみ Failure を返す。
    /// （優先度付きの OR 判定を行う制御ノード）
    /// </summary>
    public class Selector : Node
    {
        public Selector(string name) : base(name) { }

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
                        break;
                }
            }
            Reset();
            return Status.Failure;
        }
    }
}