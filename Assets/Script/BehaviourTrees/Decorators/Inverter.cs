using System;
using BlackboardSystem;

namespace BehaviourTrees
{
    public class Inverter : DecoratorNode
    {
        public Inverter(string name, Node child) : base(name, child) { }

        public override Status Process()
        {
            switch (_child.Process())
            {
                case Status.Success:
                    return Status.Failure;
                case Status.Failure:
                    return Status.Success;
                default:
                    return Status.Running;
            }
        }
    }
}