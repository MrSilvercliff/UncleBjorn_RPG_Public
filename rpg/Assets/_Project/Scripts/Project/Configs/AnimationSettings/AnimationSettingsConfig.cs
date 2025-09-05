using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Configs.AnimationSettings
{
    public interface IAnimationSettingsConfig
    {
    }

    [CreateAssetMenu(fileName = "AnimationSettingsConfig", menuName = "Project/Configs/Project/Animation Settings Config")]
    public class AnimationSettingsConfig : ScriptableObject, IAnimationSettingsConfig
    {
    }
}