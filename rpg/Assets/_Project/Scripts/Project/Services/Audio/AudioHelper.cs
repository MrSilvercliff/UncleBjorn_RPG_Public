using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts.Project.Services.Audio
{
    public static class AudioHelper
    {
        public const string MUSIC_MASTER_VOLUME = "MusicMasterVolume";
        public const string SOUND_MASTER_VOLUME = "SoundMasterVolume";

        public const float MIN_VOLUME = 0.0001f;
        public const float MAX_VOLUME = 1f;

        public static float ConvertToDB(float value)
        {
            var result = Mathf.Log10(value) * 20;
            return result;
        }

        public static void SetAudioMixerVolume(AudioMixer audioMixer, string channelKey, float volume01)
        {
            var dbVolume = ConvertToDB(volume01);
            audioMixer.SetFloat(channelKey, dbVolume);
        }
    }
}