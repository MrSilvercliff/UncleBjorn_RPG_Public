using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace _Project.Scripts.Project.Extensions
{
    public static class TransformExtensions
    {
        public static void ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void ResetLocalZ(this Transform transorm)
        {
            var localPosition = transorm.localPosition;
            localPosition.z = 0;
            transorm.localPosition = localPosition;
        }
    }
}