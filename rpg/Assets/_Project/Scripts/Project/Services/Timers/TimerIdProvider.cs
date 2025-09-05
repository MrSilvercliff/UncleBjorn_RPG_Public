using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.Project.Services.Timers
{
    public interface ITimerIdProvider
    {
    }

    public class TimerIdProvider : ITimerIdProvider
    {
        private Dictionary<string, int> _timerIdByCurrencyId;

        public TimerIdProvider()
        {
        }
    }
}