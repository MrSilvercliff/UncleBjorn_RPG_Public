using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync;
using static Codice.CM.Common.CmCallContext;

namespace _Project.Scripts.Project.Animations
{
    public interface IMonoBehaviourAnimatorController : IInitializable, IFlushable
    {
        int CurrentState { get; }

        float CurrentH { get; set; }
        float CurrentV { get; set; }
        float VelocityH { get; set; }
        float VelocityV { get; set; }

        int GetLayerIndex(string layerName);
        void Play(int stateHash, int layer = 0, int normalizedTime = 0);
        void SetFloat(string parameterName, float value);
        void SetInt(string parameterName, int value);
        void SetBool(string parameterName, bool value);
        float GetFloat(string parameterName);
        void CrossFade(int parameterName, float fadeTime, int layerName);
    }

    public class MonoBehaviourAnimatorController : MonoBehaviour, IMonoBehaviourAnimatorController
    {
        public int CurrentState => _currentStateHash;
        private float _currentH;
        private float _currentV;
        private float _velocityH;
        private float _velocityV;

        [SerializeField] private Animator _animator;

        private int _currentStateHash;

        public bool Init()
        {
            _currentStateHash = AnimatorStateHash.Empty;
            //_animator.Play(_currentStateHash, 0);
            return true;
        }

        public bool Flush()
        {
            return true;
        }

        public int GetLayerIndex(string layerName)
        {
            return _animator.GetLayerIndex(layerName);
        }

        public void Play(int stateHash, int layer = 0, int normalizedTime = 0)
        {
            _currentStateHash = stateHash;
            _animator.Play(_currentStateHash, layer, normalizedTime);
        }

        public void SetFloat(string parameterName, float value)
        {
            _animator.SetFloat(parameterName, value);
        }

        public void SetInt(string parameterName, int value)
        {
            _animator.SetInteger(parameterName, value);
        }

        public void SetBool(string parameterName, bool value)
        {
            _animator.SetBool(parameterName, value);
        }

        public float GetFloat(string parameterName)
        {
            return _animator.GetFloat(parameterName);
        }

        public void CrossFade(int parameterName, float fadeTime, int layerName)
        {
            _animator.CrossFade(parameterName, fadeTime, layerName);
        }

        public float CurrentH
        {
            get => _currentH;
            set => _currentH = value;
        }

        public float CurrentV
        {
            get => _currentV;
            set => _currentV = value;
        }

        public float VelocityH
        {
            get => _velocityH;
            set => _velocityH = value;
        }

        public float VelocityV
        {
            get => _velocityV;
            set => _velocityV = value;
        }
    }
}