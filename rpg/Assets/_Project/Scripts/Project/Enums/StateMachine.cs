using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Enums
{
    public enum StateMachineType
    { 
        NONE = 0,

        MAIN_SCENE_NULL = 1000,

        GAME_SCENE_NULL = 2000,
        CREATURE_PLAYER = 2001,
    }

    public enum StateMachineStateType
    {
        NONE = 0,
        
        CreatureState_Idle = 1,
        CreatureState_Move = 2,
        CreatureState_FreeLook = 3,
        CreatureState_RotateLook = 4,
        CreatureState_FreeFall = 5,
        CreatureState_Jump = 6,
    }
}