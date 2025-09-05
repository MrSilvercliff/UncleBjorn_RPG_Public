using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Services.Audio
{
    public interface IAudioSourceController
    {
        AudioClipId PlayingClipId { get; }
        bool IsPlaying { get; }
        void Play(IAudioClipInfo clipInfo);
        void Stop();
    }

    public class AudioSourceController : MonoBehaviour, IAudioSourceController
    {
        public AudioClipId PlayingClipId => _audioClipInfo.Id;

        public bool IsPlaying
        {
            get
            {
                if (_audioSource.loop)
                    return true;

                return _audioSource.isPlaying;
            }
        }

        [SerializeField] private AudioSource _audioSource;

        private IAudioClipInfo _audioClipInfo;

        public void Play(IAudioClipInfo audioClipInfo)
        {
            if (_audioSource == null)
                return;

            gameObject.name = $"[AudioSource] {audioClipInfo.Id}";

            _audioClipInfo = audioClipInfo;
            _audioSource.clip = audioClipInfo.AudioClip;
            _audioSource.outputAudioMixerGroup = audioClipInfo.MixerGroup;
            _audioSource.loop = audioClipInfo.Loop;
            _audioSource.Play();
        }

        public void Stop()
        {
            if (_audioSource == null)
                return;

            _audioSource.Stop();
        }
    }
}