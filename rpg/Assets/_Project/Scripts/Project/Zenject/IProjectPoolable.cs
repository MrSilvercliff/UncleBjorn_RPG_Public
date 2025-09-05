using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Project.Zenject
{
    public interface IProjectPoolable : IPoolable
    {
        void OnCreated();
    }
}