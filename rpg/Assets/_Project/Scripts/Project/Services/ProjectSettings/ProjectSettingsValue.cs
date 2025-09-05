using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Services.ProjectSettings
{
    public interface IProjectSettingsValue<T>
    {
        T Value { get; }
        void SetValue(T value);
    }

    public class ProjectSettingsValue<T> : IProjectSettingsValue<T>
    {
        public T Value => _value;

        private T _value;

        public ProjectSettingsValue(T value = default)
        {
            _value = value;
        }

        public void SetValue(T value)
        {
            _value = value;
        }
    }
}