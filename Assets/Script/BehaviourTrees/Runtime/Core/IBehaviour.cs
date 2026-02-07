namespace BehaviourTrees
{
    public interface IBehaviour
    {
        Node.Status Process();
        void Reset() { }
    }
}