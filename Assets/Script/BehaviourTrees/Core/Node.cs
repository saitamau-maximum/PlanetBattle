using BlackboardSystem;
using TMPro;

namespace BehaviourTrees
{
    public abstract class Node
    {
        public enum Status
        {
            Success,
            Failure,
            Running
        }

        public readonly string Name;

        protected Blackboard _blackboard;

        protected Node(string name = "Node")
        {
            Name = name;
        }

        public virtual void SetBlackboard(Blackboard blackboard)
        {
            _blackboard = blackboard;
        }

        public abstract Status Process();
        public abstract void Reset();
    }
}
