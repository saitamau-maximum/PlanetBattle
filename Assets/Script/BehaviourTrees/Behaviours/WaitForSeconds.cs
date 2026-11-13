using Utility;

namespace BehaviourTrees
{
    public class WaitForSeconds : Node
    {
        private TickCountdownTimer _countdownTimer;

        public WaitForSeconds(float time) : base("WaitForSeconds")
        {
            _countdownTimer = new TickCountdownTimer(time);
            _countdownTimer.Start();
        }

        public override Status Process()
        {
            _countdownTimer.Tick();

            if (_countdownTimer.IsRunning) return Status.Running;

            return Status.Success;
        }

        public override void Reset()
        {
            _countdownTimer.Start();
        }
    }
}
