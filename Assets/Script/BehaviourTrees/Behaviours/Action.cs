namespace BehaviourTrees
{
    /// <summary>
    /// Action ノード。
    /// 登録された処理（System.Action）を実行し、
    /// 処理完了後に常に Success を返す葉ノード。    
    /// </summary>

    public class Action : Node
    {
        private readonly System.Action _action;

        public Action(System.Action action)
        {
            _action = action;
        }

        public override Status Process()
        {
            _action();
            return Status.Success;
        }

        public override void Reset() { }
    }
}