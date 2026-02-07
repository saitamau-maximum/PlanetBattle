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
        public readonly int Priority;

        protected Node(string name = "Node", int priority = 0)
        {
            Name = name;
            Priority = priority;
        }

        public abstract Status Process();
        public abstract void Reset();
    }
}
