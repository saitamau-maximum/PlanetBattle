namespace BehaviourTrees
{
    public class Leaf : Node
    {
        private readonly IBehaviour _behaviour;

        public Leaf(string name, IBehaviour behaviour) : base(name)
        {
            _behaviour = behaviour;
        }

        public override Status Process()
        {
            return _behaviour.Process();
        }

        public override void Reset()
        {
            _behaviour.Reset();
        }
    }
}