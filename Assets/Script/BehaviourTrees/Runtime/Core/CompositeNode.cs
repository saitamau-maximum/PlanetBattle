using System.Collections.Generic;

namespace BehaviourTrees
{
    public abstract class CompositeNode : Node
    {
        protected readonly List<Node> _children = new();
        protected int _currentChild = 0;

        protected CompositeNode(string name, int priority = 0) : base(name, priority) { }

        public void AddChild(Node child)
        {
            _children.Add(child);
        }

        public override void Reset()
        {
            _currentChild = 0;
            foreach (var child in _children)
            {
                child.Reset();
            }
        }
    }
}
