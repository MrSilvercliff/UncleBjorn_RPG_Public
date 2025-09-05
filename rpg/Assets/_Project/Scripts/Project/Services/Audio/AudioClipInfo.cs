using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts.Project.Services.Audio
{
    public interface IAudioClipInfo
    {
        AudioClipId Id { get; }
        AudioClip AudioClip { get; }
        AudioMixerGroup MixerGroup { get; }
        bool Loop { get; }
    }

    [CreateAssetMenu(fileName = "AudioClipInfo", menuName = "Project/Configs/Project/Audio/Audio Clip Info")]
    public class AudioClipInfo : ScriptableObject, IAudioClipInfo
    {
        public AudioClipId Id => _id;
        public AudioClip AudioClip => _audioClip;
        public AudioMixerGroup MixerGroup => _mixerGroup;
        public bool Loop => _loop;
        [SerializeField] private AudioClipId _id;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] AudioMixerGroup _mixerGroup;
        [SerializeField] private bool _loop;
    }
}