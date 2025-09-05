using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Configs
{
    public interface IProjectSettingsConfig
    {
        float SoundDefaultValue { get; }
        float MusicDefaultValue { get; }
        float VolumeChangeDuration { get; }
    }

    [CreateAssetMenu(fileName = "ProjectSettingsConfig", menuName = "Project/Configs/Project/Project Settings Config")]
    public class ProjectSettingsConfig : ScriptableObject, IProjectSettingsConfig
    {
        public float SoundDefaultValue => _soundDefaultValue;
        public float MusicDefaultValue => _musicDefaultValue;
        public float VolumeChangeDuration => _volumeChangeDuration;

        [SerializeField] private float _soundDefaultValue;
        [SerializeField] private float _musicDefaultValue;
        [SerializeField] private float _volumeChangeDuration;
    }
}