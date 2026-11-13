namespace Utility
{
    public abstract class Timer
    {
        protected float _elapsedTime;
        protected bool _isRunning;

        protected virtual float ElapsedTime => _elapsedTime;

        public bool IsRunning => _isRunning && !IsFinished();
        public abstract float CurrentTime { get; }
        public abstract float Progress { get; }

        public void Start()
        {
            ResetElapsedTime(0f);
            Resume();
        }

        public abstract bool IsFinished();
        public abstract void Reset(float newTime);

        public virtual void Resume()
        {
            _isRunning = true;
        }

        public void Stop()
        {
            _elapsedTime = ElapsedTime;
            _isRunning = false;
        }

        protected void ResetElapsedTime(float elapsedTime)
        {
            _elapsedTime = elapsedTime;
            _isRunning = false;
        }
    }
}
