using System;
using UnityEngine;

namespace _Project.Scripts.Project.Services
{
    public interface IMathHelpher
    {
        float PiDegrees { get; }
        float PiDoubleDegrees { get; }
        float PiHalfDegrees { get; }
        float PiQuarterDegrees { get; }

        float ConvertToDegrees(float radian);
        float ConvertToRadian(float degrees);
    }

    public class MathHelper : IMathHelpher
    {
        public float PiDegrees => PI_DEGREES;
        public float PiDoubleDegrees => PI_DOUBLE_DEGREES;
        public float PiHalfDegrees => PI_HALF_DEGREES;
        public float PiQuarterDegrees => PI_QUARTER_DEGREES;
        
        private const float PI_DOUBLE_DEGREES = 360f;
        private const float PI_DEGREES = 180f;
        private const float PI_HALF_DEGREES = 90f;
        private const float PI_QUARTER_DEGREES = 45f;

        public float ConvertToDegrees(float radian)
        {
            var result = (float)((radian * 180f) / Math.PI);
            return result;
        }

        public float ConvertToRadian(float degrees)
        {
            var result = (float)((degrees * Math.PI) / 180);
            return result;
        }
    }
}
