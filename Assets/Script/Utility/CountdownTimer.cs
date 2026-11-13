using UnityEngine;

namespace Utility
{
    /// <summary>
    /// CountdownTimer は指定した時間からカウントダウンを行うタイマーです。
    /// <para>Time.time に基づく経過時間から残り時間を計算するため、Tick() は不要です。</para>
    /// <para>生成直後は完了扱いです。Start() で計測を開始します。</para>
    /// </summary>
    public class CountdownTimer : Timer
    {
        private float _initialTime;
        private float _startedAt;

        protected override float ElapsedTime => _isRunning
            ? _elapsedTime + (Time.time - _startedAt)
            : _elapsedTime;

        public override float CurrentTime => Mathf.Max(0f, _initialTime - ElapsedTime);
        public override float Progress => _initialTime <= 0f ? 1f : Mathf.Clamp01(CurrentTime / _initialTime);

        /// <summary>
        /// 指定した時間で CountdownTimer を生成します。
        /// </summary>
        /// <param name="time">カウントダウンする秒数</param>
        public CountdownTimer(float time)
        {
            _initialTime = Mathf.Max(0f, time);
            // 生成直後は完了扱いにし、Start() で指定時間の計測を開始する。
            ResetElapsedTime(_initialTime);
        }

        public override void Resume()
        {
            if (_isRunning) return;

            _startedAt = Time.time;
            base.Resume();
        }

        public override void Reset(float newTime)
        {
            _initialTime = Mathf.Max(0f, newTime);
            ResetElapsedTime(0f);
        }

        /// <summary>
        /// タイマーが終了しているかどうかを判定します。
        /// </summary>
        /// <returns>CurrentTime が 0 以下の場合に true を返します。</returns>
        public override bool IsFinished() => CurrentTime <= 0;
    }
}
