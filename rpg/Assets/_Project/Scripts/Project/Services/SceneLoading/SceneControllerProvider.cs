using _Project.Scripts.Project.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Services.SceneLoading
{
    public interface ISceneControllerProvider
    {
        ISceneController CurrentSceneController { get; }
        void SetCurrentSceneController(ISceneController controller);
    }

    public class SceneControllerProvider : ISceneControllerProvider
    {
        public ISceneController CurrentSceneController => _controller;

        private ISceneController _controller;

        public void SetCurrentSceneController(ISceneController controller)
        {
            _controller = controller;
        }
    }
}