using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Enums
{
    public enum CreatureType
    { 
        NONE = 0,
        PLAYER = 1,
    }

    public enum CreatureComponentType
    { 
        None = 0,
        Move = 1,
        CameraLook = 2,
        CharacterController = 3,
        StateMachineDebug = 4,
        AnimatorController = 5,
    }
}