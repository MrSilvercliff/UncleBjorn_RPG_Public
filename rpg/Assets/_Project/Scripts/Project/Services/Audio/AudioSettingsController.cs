using _Project.Scripts.Project.Configs;
using _Project.Scripts.Project.Configs.Audio;
using _Project.Scripts.Project.Dotween;
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
    public interface IAudioSettingsController : IProjectSerivce
    {
        void SetMusicVolume(float value);
        void SetSoundVolume(float value);
    }

    public class AudioSettingsController : IAudioSettingsController
    {
        [Inject] private IAudioConfig _config;
        [Inject] private IProjectSettingsConfig _gameSettingsConfig;

        private AudioMixer _mixer;

        private Sequence _musicSequence;
        private Sequence _soundSequence;

        public Task<bool> Init()
        {
            _mixer = _config.AudioMixer;
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        public void SetMusicVolume(float value)
        {
            DotweenHelper.KillSequence(_musicSequence);
            _musicSequence = PlayVolumeChangeSequence(AudioHelper.MUSIC_MASTER_VOLUME, value);
        }

        public void SetSoundVolume(float value)
        {
            DotweenHelper.KillSequence(_soundSequence);
            _soundSequence = PlayVolumeChangeSequence(AudioHelper.SOUND_MASTER_VOLUME, value);
        }

        private Sequence PlayVolumeChangeSequence(string volumeKey, float value01)
        {
            var fromValue = value01 > AudioHelper.MIN_VOLUME ? AudioHelper.MIN_VOLUME : AudioHelper.MAX_VOLUME;
            var duration = _gameSettingsConfig.VolumeChangeDuration;

            var seq = DOTween.Sequence();
            seq.Append(DOTween.To(x => { AudioHelper.SetAudioMixerVolume(_mixer, volumeKey, x); }, fromValue, value01, duration));
            seq.Play();
            return seq;
        }
    }
}