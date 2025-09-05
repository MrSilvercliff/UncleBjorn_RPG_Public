using _Project.Scripts.GameScene.Creatures.Basis;
using _Project.Scripts.Project.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Animations
{
    public class CreatureAnimationFinishHandler : AnimationFinishHandler
    {
        private ICreatureController _creatureController;

        public void Setup(ICreatureController creatureController)
        {
            _creatureController = creatureController;
        }

        public override void OnAnimationFinish()
        {
            Debug.LogError($"[{GetType().Name}] OnAnimationFinish");
        }

        public override void OnAnimationFinishInt(int value)
        {
            Debug.LogError($"[{GetType().Name}] OnAnimationFinish int");
        }

        public override void OnAnimationFinishFloat(float value)
        {
            Debug.LogError($"[{GetType().Name}] OnAnimationFinish float");
        }

        public override void OnAnimationFinishString(string stringValue)
        {
            //Debug.LogError($"[{GetType().Name}] OnAnimationFinish string");
            var stateHash = Animator.StringToHash(stringValue);
            _creatureController.OnAnimationFinished(stateHash);
        }
    }
}