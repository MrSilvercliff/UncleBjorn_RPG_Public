using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Animations
{
    public static class AnimatorStateHash
    {
        public static readonly int NONE = Animator.StringToHash("");

        public static readonly int Empty = Animator.StringToHash("Empty");

        public static readonly int Idle = Animator.StringToHash("Idle");

        public static readonly int TurnLeft = Animator.StringToHash("TurnLeft");
        public static readonly int TurnRight= Animator.StringToHash("TurnRight");

        public static readonly int RunLeft = Animator.StringToHash("RunLeft");
        public static readonly int RunRight = Animator.StringToHash("RunRight");
        
        public static readonly int RunForward = Animator.StringToHash("RunForward");
        public static readonly int RunForwardLeft = Animator.StringToHash("RunForwardLeft");
        public static readonly int RunForwardRight = Animator.StringToHash("RunForwardRight");
        
        public static readonly int RunBackward = Animator.StringToHash("RunBackward");
        public static readonly int RunBackwardLeft = Animator.StringToHash("RunBackwardLeft");
        public static readonly int RunBackwardRight = Animator.StringToHash("RunBackwardRight");

        public static readonly int JumpStart = Animator.StringToHash("JumpStart");
        public static readonly int JumpFall = Animator.StringToHash("JumpFall");
        public static readonly int JumpGrounding = Animator.StringToHash("JumpGrounding");

        public static readonly int Blending = Animator.StringToHash("Blend Tree");
    }
}