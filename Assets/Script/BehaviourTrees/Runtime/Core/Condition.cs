using System;
using UnityEngine;

namespace BehaviourTrees
{
    public abstract class Condition : Node
    {
        public Condition(string name) : base(name) { }

        protected abstract bool Check();

        public override Status Process()
        {
            Debug.Log($"Processing Sequence: {Name} {Check()}");
            return Check() ? Status.Success : Status.Failure;
        }

        public override void Reset() { }
    }
}