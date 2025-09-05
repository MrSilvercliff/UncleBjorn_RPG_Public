using _Project.Scripts.Project.Configs;
using _Project.Scripts.Project.Configs.Audio;
using _Project.Scripts.Project.Dotween;
using _Project.Scripts.Project.Enums;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;

namespace _Project.Scripts.Project.Services.Audio
{
    public interface IAudioMixingController : IProjectSerivce
    {
        void SwitchMusicAudioMixerGroup(AudioMixerGroupId targetMixerGroupId);
    }

    public class AudioMixingController : IAudioMixingController
    {
        [Inject] private IAudioConfig _audioConfig;
        [Inject] private IProjectSettingsConfig _gameSettingsConfig;

        private AudioMixer _audioMixer;
        private Dictionary<AudioMixerGroupId, float> _lastVolumes;

        private Sequence _switchSequence;

        public AudioMixingController()
        {
            _switchSequence = null;
            _lastVolumes = new();
        }

        public Task<bool> Init()
        {
            _audioMixer = _audioConfig.AudioMixer;
            InitAudioMixerMusicGroups();
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        private void InitAudioMixerMusicGroups()
        {
            var musicGroups = _audioConfig.AudioMixerMusicGroupInfos;

            var minVolume = AudioHelper.MIN_VOLUME;

            foreach (var musicGroupInfo in musicGroups)
            {
                var volumeExposeParameterName = musicGroupInfo.VolumeExposeParameterName;
                AudioHelper.SetAudioMixerVolume(_audioMixer, volumeExposeParameterName, minVolume);
                _lastVolumes[musicGroupInfo.Id] = minVolume;
            }
        }

        public void SwitchMusicAudioMixerGroup(AudioMixerGroupId targetMixerGroupId)
        {
            var targetMixerGroupInfo = _audioConfig.GetAudioMixerGroupById(targetMixerGroupId);
            var allMixerGroups = _audioConfig.AudioMixerMusicGroupInfos;

            var volumeKey = targetMixerGroupInfo.VolumeExposeParameterName;
            var volumeChangeDuration = _gameSettingsConfig.VolumeChangeDuration;
            var minVolume = AudioHelper.MIN_VOLUME;
            var maxVolume = AudioHelper.MAX_VOLUME;

            DotweenHelper.KillSequence(_switchSequence);

            _switchSequence = DOTween.Sequence();

            var lastVolume = _lastVolumes[targetMixerGroupId];

            if (lastVolume < maxVolume)
            {
                _switchSequence.Insert(0f, DOTween.To(x => { AudioHelper.SetAudioMixerVolume(_audioMixer, volumeKey, x); }, minVolume, maxVolume, volumeChangeDuration));
                _lastVolumes[targetMixerGroupId] = maxVolume;
            }

            foreach (var musicMixerGroupInfo in allMixerGroups)
            {
                var id = musicMixerGroupInfo.Id;

                if (id == targetMixerGroupId)
                    continue;

                lastVolume = _lastVolumes[id];

                if (lastVolume <= minVolume)
                    continue;

                _lastVolumes[id] = minVolume;
                var tweenTargetVolumeKey = musicMixerGroupInfo.VolumeExposeParameterName;
                _switchSequence.Insert(0f, DOTween.To(x => { AudioHelper.SetAudioMixerVolume(_audioMixer, tweenTargetVolumeKey, x); }, maxVolume, minVolume, volumeChangeDuration));
            }

            _switchSequence.Play();
        }
    }
}