using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.Balance.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Abilities
{
    public interface IAbility
    { 
        string Id { get; }
        AbilityType AbilityType { get; }
    }

    public abstract class Ability<TFactoryType> : IAbility
        where TFactoryType : IAbility
    {
        public string Id => _balanceModel.Id;
        public AbilityType AbilityType => _balanceModel.AbilityType;

        protected IAbilityBalanceModel _balanceModel;

        public Ability(IAbilityBalanceModel balanceModel)
        {
            _balanceModel = balanceModel;
        }

        public class Factory : PlaceholderFactory<IAbilityBalanceModel, TFactoryType> { }
    }
}