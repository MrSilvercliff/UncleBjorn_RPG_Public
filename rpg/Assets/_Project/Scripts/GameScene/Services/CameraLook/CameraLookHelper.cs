using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Services.CameraLook
{
    public static class CameraLookHelper
    {
        private const int PI_HALF_DEGREES = 180;

        public static Vector3 ApplyLookInput(Vector3 cameraEulerAngles, Vector2 lookInput)
        {
            var x = CalculateEulerX(cameraEulerAngles.x, lookInput.y);
            var y = CalculateEulerY(cameraEulerAngles.y, lookInput.x);
            var result = new Vector3(x, y, 0);
            return result;
        }

        private static float CalculateEulerX(float cameraEulerX, float lookInput)
        {
            var result = cameraEulerX - lookInput;
            var math = -(result - PI_HALF_DEGREES);

            if (math > 0 && math < 90f)
                result = 90f;

            if (math < 0 && math > -90f)
                result = -90f;

            return result;
        }

        private static float CalculateEulerY(float cameraEulerY, float lookInput)
        { 
            var result = cameraEulerY + lookInput;
            return result;
        }
    }
}