using System;
using UnityEngine;

namespace Utility
{
    public abstract class Timer
    {
        private float _initalTime;
        public float CurrentTime { get; protected set; }
        public bool IsRunning { get; private set; }
        public float Progress => (_initalTime <= 0) ? 1f : Mathf.Clamp(CurrentTime / _initalTime, 0f, 1f);

        protected Timer(float time)
        {
            _initalTime = time;
        }
        public void Start()
        {
            CurrentTime = _initalTime;
            IsRunning = true;
        }

        public abstract void Tick();

        public abstract bool IsFinished();

        public void Resume() => IsRunning = true;
        public void Stop() => IsRunning = false;

        public void Reset(float newTime)
        {
            _initalTime = newTime;
            CurrentTime = newTime;
            IsRunning = false;
        }
    }
}