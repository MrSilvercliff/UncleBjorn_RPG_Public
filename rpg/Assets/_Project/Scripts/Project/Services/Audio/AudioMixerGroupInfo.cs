using _Project.Scripts.Project.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts.Project.Services.Audio
{
    public interface IAudioMixerGroupInfo
    {
        AudioMixerGroupId Id { get; }
        AudioMixerGroup MixerGroup { get; }
        string VolumeExposeParameterName { get; }
    }

    [Serializable]
    public class AudioMixerGroupInfo : IAudioMixerGroupInfo
    {
        public AudioMixerGroupId Id => _id;
        public AudioMixerGroup MixerGroup => _mixerGroup;
        public string VolumeExposeParameterName => _volumeExposeParameterName;

        [SerializeField] private AudioMixerGroupId _id;
        [SerializeField] private AudioMixerGroup _mixerGroup;
        [SerializeField] private string _volumeExposeParameterName;
    }
}