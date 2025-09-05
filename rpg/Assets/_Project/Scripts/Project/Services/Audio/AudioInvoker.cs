using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Project.Services.Audio
{
    public class AudioInvoker : MonoBehaviour
    {
        [Inject] private IAudioPlayController _audioPlayController;

        public void Play(AudioClipInfo audioClipInfo)
        {
            _audioPlayController.Play(audioClipInfo);
        }
    }
}