using System.Collections.Generic;

namespace BehaviourTrees
{
    public class Node
    {
        public enum Status
        {
            Success,
            Failure,
            Running
        }
        public readonly string Name;

        protected readonly List<Node> _children = new();
        protected int _currentChild = 0;

        public Node(string name = "Node")
        {
            Name = name;
        }

        public void AddChild(Node child)
        {
            _children.Add(child);
        }

        public virtual Status Process()
        {
            return _children[_currentChild].Process();
        }

        public virtual void Reset()
        {
            _currentChild = 0;
            foreach (var child in _children)
            {
                child.Reset();
            }
        }
    }
}