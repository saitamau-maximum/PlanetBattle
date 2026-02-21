using BlackboardSystem;

namespace BehaviourTrees
{
    public class BehaviourTree : CompositeNode
    {
        public BehaviourTree(string name = "behaviourTree") : base(name) { }

        public override Status Process()
        {
            while (_currentChild < _children.Count)
            {
                Node.Status status = _children[_currentChild].Process();
                if (status != Status.Success)
                {
                    return status;
                }
                _currentChild++;
            }
            return Status.Success;
        }
    }
}