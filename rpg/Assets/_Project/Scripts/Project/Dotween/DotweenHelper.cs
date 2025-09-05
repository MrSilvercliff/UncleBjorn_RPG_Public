using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Dotween
{
    public static class DotweenHelper
    {
        public static void KillSequence(Sequence sequence, bool complete = false)
        {
            if (sequence == null)
                return;

            if (!sequence.IsActive())
                return;

            if (!sequence.IsPlaying())
                return;

            sequence.Kill(complete);
        }
    }
}