using Utility;
using UnityEngine;

namespace BehaviourTrees
{
    public class WaitForSeconds : Node
    {
        private CountdownTimer _countdownTimer;

        public WaitForSeconds(float time) : base("WaitForSeconds")
        {
            _countdownTimer = new CountdownTimer(time);
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