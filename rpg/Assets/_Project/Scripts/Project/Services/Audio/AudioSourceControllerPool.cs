using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Project.Services.Audio
{
    public class AudioSourceControllerPool : MonoMemoryPool<AudioSourceController>
    {
    }
}