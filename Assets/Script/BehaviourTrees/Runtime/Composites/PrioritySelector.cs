using System.Collections.Generic;
using System.Linq;

namespace BehaviourTrees
{
    public class PrioritySelector : Selector
    {
        private List<Node> _sorderedChildren;
        private List<Node> SortedChildren => _sorderedChildren ??= _children.OrderByDescending(child => child.Priority).ToList();

        public PrioritySelector(string name) : base(name) { }

        public override Status Process()
        {
            foreach (Node child in SortedChildren)
            {
                switch (child.Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        return Status.Success;
                    default:
                        continue;
                }
            }

            return Status.Failure;
        }

        public override void Reset()
        {
            base.Reset();
            _sorderedChildren = null;
        }
    }
}