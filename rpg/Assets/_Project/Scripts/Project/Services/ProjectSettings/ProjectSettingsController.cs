using _Project.Scripts.Project.Configs;
using _Project.Scripts.Project.Services.Audio;
using System.Threading.Tasks;
using Plugins.ZerglingUnityPlugins.Localization_JSON_Object.Scripts;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;

namespace _Project.Scripts.Project.Services.ProjectSettings
{
    public interface IProjectSettingsController : IProjectSerivce
    {
        bool MusicEnabled { get; }
        bool SoundEnabled { get; }

        void ToggleMusic();
        void ToggleSound();
        void ChangeLanguage(SystemLanguage language);
    }

    public class ProjectSettingsController : IProjectSettingsController
    {
        public bool MusicEnabled
        {
            get
            {
                var value = _music.Value;
                return value > AudioHelper.MIN_VOLUME;
            }
        }

        public bool SoundEnabled
        {
            get
            {
                var value = _sound.Value;
                return value > AudioHelper.MIN_VOLUME;
            }
        }

        [Inject] private IProjectSettingsConfig _config;
        [Inject] private IAudioSettingsController _audioSettingsController;
        [Inject] private ILocalizationService _localizationService;

        private IProjectSettingsValue<float> _sound;
        private IProjectSettingsValue<float> _music;
        private IProjectSettingsValue<SystemLanguage> _gameLanguage;

        public ProjectSettingsController()
        {
            _sound = new ProjectSettingsValue<float>(0);
            _music = new ProjectSettingsValue<float>(0);
            _gameLanguage = new ProjectSettingsValue<SystemLanguage>(SystemLanguage.Unknown);
        }

        public Task<bool> Init()
        {
            _sound.SetValue(_config.SoundDefaultValue);
            _music.SetValue(_config.MusicDefaultValue);
            _gameLanguage.SetValue(_localizationService.CurrentLanguage);
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        public void ToggleMusic()
        {
            var value = _music.Value;
            var newValue = value > AudioHelper.MIN_VOLUME ? AudioHelper.MIN_VOLUME : AudioHelper.MAX_VOLUME;
            _music.SetValue(newValue);
            _audioSettingsController.SetMusicVolume(newValue);
        }

        public void ToggleSound()
        {
            var value = _sound.Value;
            var newValue = value > AudioHelper.MIN_VOLUME ? AudioHelper.MIN_VOLUME : AudioHelper.MAX_VOLUME;
            _sound.SetValue(newValue);
            _audioSettingsController.SetSoundVolume(newValue);
        }

        public void ChangeLanguage(SystemLanguage language)
        {
            _localizationService.ChangeLanguage(language);
            _gameLanguage.SetValue(language);
        }
    }
}