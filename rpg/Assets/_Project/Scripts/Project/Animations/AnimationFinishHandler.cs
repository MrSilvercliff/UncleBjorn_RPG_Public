using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Animations
{
    public abstract class AnimationFinishHandler : MonoBehaviour
    {
        public abstract void OnAnimationFinish();
        public abstract void OnAnimationFinishInt(int value);
        public abstract void OnAnimationFinishFloat(float value);
        public abstract void OnAnimationFinishString(string value);
    }
}