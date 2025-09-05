using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZerglingUnityPlugins.Timers.Scripts
{
    public interface ISecondTickTimer : ITimer
    {
        event Action<ISecondTickTimer> TickEvent;
    }

    public class SecondTickTimer : Timer, ISecondTickTimer
    {
        public event Action<ISecondTickTimer> TickEvent;

        private float _secondProgress;

        public SecondTickTimer(int id) : base(id)
        {
        }

        public override void OnProcess(float deltaTime)
        {
            base.OnProcess(deltaTime);

            if (Paused)
                return;

            if (Expired)
                return;

            _secondProgress += deltaTime;

            if (_secondProgress < 1)
                return;

            TickEvent?.Invoke(this);
            _secondProgress = 0f;
        }
    }
}