namespace BehaviourTrees
{
    public abstract class DecoratorNode : Node
    {
        protected readonly Node _child;

        protected DecoratorNode(string name, Node child, int priority = 0) : base(name, priority)
        {
            _child = child;
        }

        public override void Reset()
        {
            _child.Reset();
        }
    }
}
