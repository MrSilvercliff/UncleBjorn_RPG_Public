using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZerglingUnityPlugins.Timers.Scripts
{
    public interface ITimerControllerBase
    {
        void StartTimer<T>(T timer, float duration) where T : ITimer;

        void PauseTimer(ITimer timer, bool paused);
        void PauseTimer(int id, bool paused);

        void StopTimer(ITimer timer, bool reset);
        void StopTimer(int id, bool reset);

        void ProcessTimers(float deltaTime);
    }

    public class TimerControllerBase : ITimerControllerBase
    {
        private List<ITimer> _timersList;
        private Dictionary<int, ITimer> _timersById;

        private Dictionary<ITimer, bool> _timersToChange;

        public TimerControllerBase()
        {
            _timersList = new List<ITimer>();
            _timersById = new Dictionary<int, ITimer>();

            _timersToChange = new Dictionary<ITimer, bool>();
        }

        public void StartTimer<T>(T timer, float duration) where T : ITimer
        {
            timer.Start(duration);
            _timersToChange[timer] = true;
        }

        public void PauseTimer(ITimer timer, bool paused)
        {
            timer.Pause(paused);
        }

        public void PauseTimer(int id, bool paused)
        {
            var timer = _timersById[id];
            timer.Pause(paused);
        }

        public void StopTimer(ITimer timer, bool reset)
        {
            timer.Stop(reset);
            _timersToChange[timer] = false;
        }

        public void StopTimer(int id, bool reset)
        {
            var timer = _timersById[id];
            timer.Stop(reset);
            _timersToChange[timer] = false;
        }

        public void ProcessTimers(float deltaTime)
        {
            CheckTimersToChange();

            foreach (var timer in _timersList)
            {
                timer.OnProcess(deltaTime);

                if (!timer.Expired)
                    continue;

                if (_timersToChange.ContainsKey(timer))
                    continue;

                StopTimer(timer, false);
            }
        }

        private void CheckTimersToChange()
        {
            if (_timersToChange.Count == 0)
                return;

            foreach (var timerItem in _timersToChange)
            {
                var timer = timerItem.Key;
                var changeValue = timerItem.Value;

                if (changeValue)
                    AddTimer(timer);
                else
                    RemoveTimer(timer);
            }

            _timersToChange.Clear();
        }

        private void AddTimer(ITimer timer)
        {
            var id = timer.Id;
            
            if (_timersById.ContainsKey(id))
                return;

            _timersById[id] = timer;
            _timersList.Add(timer);
        }

        private void RemoveTimer(ITimer timer)
        {
            var id = timer.Id;

            if (!_timersById.ContainsKey(id))
                return;

            _timersById.Remove(id);
            _timersList.Remove(timer);
        }
    }
}