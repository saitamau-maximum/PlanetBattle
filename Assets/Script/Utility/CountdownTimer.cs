using UnityEngine;

namespace Utility
{
    /// <summary>
    /// CountdownTimer は指定した時間からカウントダウンを行うタイマーです。
    /// <para>Time.deltaTime を使用して毎フレーム減算されます。</para>
    /// <para>タイマーがゼロになると自動で停止します。</para>
    /// </summary>
    public class CountdownTimer : Timer
    {
        /// <summary>
        /// 指定した時間で CountdownTimer を生成します。
        /// </summary>
        /// <param name="time">カウントダウンする秒数</param>
        public CountdownTimer(float time) : base(time) { }

        /// <summary>
        /// タイマーを 1 フレーム分進めます。
        /// <para>IsRunning が true の場合のみ減算されます。</para>
        /// タイマーが 0 以下になると自動的に停止します。
        /// </summary>
        public override void Tick()
        {
            if (IsRunning && CurrentTime > 0)
            {
                CurrentTime -= Time.deltaTime;
            }
            if (CurrentTime <= 0)
            {
                Stop();
            }
        }

        /// <summary>
        /// タイマーが終了しているかどうかを判定します。
        /// </summary>
        /// <returns>CurrentTime が 0 以下の場合に true を返します。</returns>
        public override bool IsFinished() => CurrentTime <= 0;
    }
}
