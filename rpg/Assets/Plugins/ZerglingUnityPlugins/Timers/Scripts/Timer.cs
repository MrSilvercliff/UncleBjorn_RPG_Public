using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

namespace ZerglingUnityPlugins.Timers.Scripts
{
    public interface ITimer
    {
        event Action<ITimer> ExpiredEvent;

        int Id { get; }

        bool Expired { get; }
        bool Paused { get; }
        float Progress { get; }
        float Duration { get; }
        float TimeSpend { get; }
        float TimeLeft { get; }
        int SecondsSpend { get; }
        int SecondsLeft { get; }

        void Start(float duration);
        void OnProcess(float deltaTime);
        void Pause(bool paused);
        void Continue(float timeSpend);
        void Continue(float timeSpend, float duration);
        void Stop(bool reset);
        void Reset();
        void SetTimeSpend(float value);
    }

    public class Timer : ITimer
    {
        public event Action<ITimer> ExpiredEvent;

        public int Id { get; }

        public bool Expired { get; protected set; }
        public bool Paused { get; private set; }
        public float Progress => GetProgress();
        public float Duration { get; private set; }
        public float TimeSpend { get; protected set; }
        public float TimeLeft => GetTimeLeft();
        public int SecondsSpend => GetSecondsSpend();
        public int SecondsLeft => GetSecondsLeft();

        public Timer(int id)
        {
            Id = id;
            Expired = true;
            Duration = 0f;
            TimeSpend = 0f;
        }

        public void Start(float duration)
        {
            Continue(0f, duration);
        }

        public virtual void OnProcess(float deltaTime)
        {
            if (Paused)
                return;

            if (Expired)
                return;

            var newTimeSpend = TimeSpend + deltaTime;
            SetTimeSpend(newTimeSpend);
        }

        public void Pause(bool paused)
        {
            Paused = paused;
        }

        public void Continue(float timeSpend)
        {
            SetTimeSpend(timeSpend);
        }

        public void Continue(float timeSpend, float duration)
        {
            Duration = Math.Max(duration, 0f);
            SetTimeSpend(timeSpend);
        }

        public void Stop(bool reset)
        {
            Expired = true;

            if (reset)
                Reset();
        }

        public void Reset()
        {
            Expired = false;
            Duration = 0f;
            TimeSpend = 0f;
        }

        public void SetTimeSpend(float value)
        {
            TimeSpend = value;

            if (TimeSpend >= Duration)
            {
                if (Expired)
                    return;

                Expired = true;
                SendExpiredEvent();
            }
            else
                Expired = false;
        }

        protected float GetProgress()
        {
            if (Duration > 0f)
            {
                var result = Mathf.Clamp01(TimeSpend / Duration);
                return result;
            }

            return 0f;
        }

        protected float GetTimeLeft()
        {
            if (Expired)
                return 0f;

            var result = Duration - TimeSpend;
            return result;
        }

        protected int GetSecondsSpend()
        {
            var result = Mathf.FloorToInt(TimeSpend);
            return result;
        }

        protected int GetSecondsLeft()
        {
            var result = Mathf.CeilToInt(TimeLeft);
            return result;
        }

        protected void SendExpiredEvent()
        {
            ExpiredEvent?.Invoke(this);
        }
    }
}