namespace BehaviourTrees
{
    /// <summary>
    /// Action ノード。
    /// 登録された処理（System.Action）を実行し、
    /// 処理完了後に常に Success を返す葉ノード。    
    /// </summary>

    public class Action : IBehaviour
    {
        private readonly System.Action _action;

        public Action(System.Action action)
        {
            _action = action;
        }

        public Node.Status Process()
        {
            _action();
            return Node.Status.Success;
        }
    }
}