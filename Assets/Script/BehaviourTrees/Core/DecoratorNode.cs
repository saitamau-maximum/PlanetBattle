using BlackboardSystem;

namespace BehaviourTrees
{
    public abstract class DecoratorNode : Node
    {
        protected readonly Node _child;

        protected DecoratorNode(string name, Node child) : base(name)
        {
            _child = child;
        }

        public override void SetBlackboard(Blackboard blackboard)
        {
            _child.SetBlackboard(blackboard);
        }

        public override void Reset()
        {
            _child.Reset();
        }
    }
}
