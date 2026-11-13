using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Tick() を呼び出した分だけ時間が進むカウントダウンタイマーです。
    /// 生成直後は完了扱いです。Start() で計測を開始します。
    /// </summary>
    public class TickCountdownTimer : Timer
    {
        private float _initialTime;

        public override float CurrentTime => Mathf.Max(0f, _initialTime - ElapsedTime);
        public override float Progress => _initialTime <= 0f ? 1f : Mathf.Clamp01(CurrentTime / _initialTime);

        public TickCountdownTimer(float time)
        {
            _initialTime = Mathf.Max(0f, time);
            ResetElapsedTime(_initialTime);
        }

        public void Tick()
        {
            Tick(Time.deltaTime);
        }

        public void Tick(float deltaTime)
        {
            if (!IsRunning) return;

            _elapsedTime += Mathf.Max(0f, deltaTime);
            if (IsFinished()) Stop();
        }

        public override bool IsFinished() => CurrentTime <= 0f;

        public override void Reset(float newTime)
        {
            _initialTime = Mathf.Max(0f, newTime);
            ResetElapsedTime(0f);
        }
    }
}
