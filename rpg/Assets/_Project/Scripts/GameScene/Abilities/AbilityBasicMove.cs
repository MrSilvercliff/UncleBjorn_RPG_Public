using _Project.Scripts.Project.Services.Balance.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Abilities
{
    public interface IAbilityBasicMove : IAbility
    { 
        float MoveSpeedForward { get; }
        float MoveSpeedBackward { get; }
        float MoveSpeedStrafe { get; }
        float JumpHeight { get; }
    }

    public class AbilityBasicMove : Ability<AbilityBasicMove>, IAbilityBasicMove
    {
        public float MoveSpeedForward => _balanceModel.FloatValue1;
        public float MoveSpeedBackward => _balanceModel.FloatValue2;
        public float MoveSpeedStrafe => _balanceModel.FloatValue3;
        public float JumpHeight => _balanceModel.FloatValue4;

        public AbilityBasicMove(IAbilityBalanceModel balanceModel) : base(balanceModel)
        {
        }
    }
}