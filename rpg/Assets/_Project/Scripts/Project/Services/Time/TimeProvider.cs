using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Services.Time
{
    public interface ITimeProvider
    {
        DateTime UtcNow { get; }

        DateTime GetTodayMidnight();
        DateTime GetTomorrowMidnight();
    }

    public class TimeProvider : ITimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime GetTodayMidnight()
        {
            var utcNow = UtcNow;

            var result = utcNow.AddHours(-utcNow.Hour);
            result = result.AddMinutes(-utcNow.Minute);
            result = result.AddSeconds(-utcNow.Second);
            result = result.AddMilliseconds(-utcNow.Millisecond);

            return result;
        }

        public DateTime GetTomorrowMidnight()
        {
            var utcNow = UtcNow;

            var result = utcNow.AddDays(1);
            result = result.AddHours(-utcNow.Hour);
            result = result.AddMinutes(-utcNow.Minute);
            result = result.AddSeconds(-utcNow.Second);
            result = result.AddMilliseconds(-utcNow.Millisecond);

            return result;
        }
    }
}