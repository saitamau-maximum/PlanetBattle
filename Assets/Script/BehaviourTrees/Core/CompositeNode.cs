using System.Collections.Generic;
using BlackboardSystem;

namespace BehaviourTrees
{
    public abstract class CompositeNode : Node
    {
        protected List<Node> _children = new();
        protected int _currentChild = 0;

        protected CompositeNode(string name = "CompositeNode") : base(name) { }

        public virtual void AddChild(Node child)
        {
            _children.Add(child);
        }

        public override void SetBlackboard(Blackboard blackboard)
        {
            foreach (Node node in _children)
            {
                node.SetBlackboard(blackboard);
            }
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
