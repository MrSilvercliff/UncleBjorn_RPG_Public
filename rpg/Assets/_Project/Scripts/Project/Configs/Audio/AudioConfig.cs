using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using ZerglingUnityPlugins.Tools.Scripts.Configs;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.Project.Configs.Audio
{
    public interface IAudioConfig : IConfigBase
    {
        AudioMixer AudioMixer { get; }
        IReadOnlyCollection<IAudioMixerGroupInfo> AudioMixerMusicGroupInfos { get; }

        IAudioMixerGroupInfo GetAudioMixerGroupById(AudioMixerGroupId id);
        IAudioClipInfo GetById(AudioClipId id);
    }

    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Project/Configs/Project/Audio/Audio Config")]
    public class AudioConfig : ConfigBase<AudioConfig>, IAudioConfig
    {
        public AudioMixer AudioMixer => _audioMixer;
        public IReadOnlyCollection<IAudioMixerGroupInfo> AudioMixerMusicGroupInfos => _audioMixerMusicGroupInfos;

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerGroupInfo[] _audioMixerMusicGroupInfos;
        [SerializeField] private AudioClipInfo[] _audioClipInfos;

        private Dictionary<AudioMixerGroupId, IAudioMixerGroupInfo> _audioMixerMusicGroupInfoInfoById;
        private Dictionary<AudioClipId, IAudioClipInfo> _audioClipInfoById;

        public override void Init()
        {
            base.Init();
            CacheAudioMixerChannels();
            CacheAudioClips();
        }

        private void CacheAudioMixerChannels()
        {
            _audioMixerMusicGroupInfoInfoById = new();

            foreach (var audioMixerGroupInfo in _audioMixerMusicGroupInfos)
            {
                var id = audioMixerGroupInfo.Id;
                _audioMixerMusicGroupInfoInfoById[id] = audioMixerGroupInfo;
            }
        }

        private void CacheAudioClips()
        {
            _audioClipInfoById = new Dictionary<AudioClipId, IAudioClipInfo>();

            foreach (var audioClipInfo in _audioClipInfos)
            {
                var id = audioClipInfo.Id;
                _audioClipInfoById[id] = audioClipInfo;
            }
        }

        public IAudioMixerGroupInfo GetAudioMixerGroupById(AudioMixerGroupId id)
        {
            var tryGetResult = _audioMixerMusicGroupInfoInfoById.TryGetValue(id, out var result);

            if (!tryGetResult)
                LogUtils.Error(this, $"Audio mixer group info with id [{id}] does not exist!");

            return result;
        }

        public IAudioClipInfo GetById(AudioClipId id)
        {
            var tryGetResult = _audioClipInfoById.TryGetValue(id, out var result);

            if (!tryGetResult)
                LogUtils.Error(this, $"Audio clip info with id [{id}] does not exist!");

            return result;
        }
    }
}