using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures.Player.States
{
    public interface IPlayerStateLook : IPlayerStateControllerBase
    {
        void OnLook(Vector2 lookInput);
    }
}