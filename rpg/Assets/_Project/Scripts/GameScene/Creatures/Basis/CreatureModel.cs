using _Project.Scripts.GameScene.Abilities;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.Balance.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Basis
{
    public interface ICreatureModel
    {
        StateMachineType StateMachineType { get; }

        void Setup(IReadOnlyDictionary<AbilityType, IAbility> abilities);
        T GetAbility<T>(AbilityType abilityType) where T : IAbility;
    }

    public class CreatureModel : ICreatureModel
    {
        public StateMachineType StateMachineType => _balanceModel.StateMachineType;

        private ICreatureBalanceModel _balanceModel;
        private IReadOnlyDictionary<AbilityType, IAbility> _abilities;

        public CreatureModel(ICreatureBalanceModel balanceModel)
        {
            _balanceModel = balanceModel;
        }

        public void Setup(IReadOnlyDictionary<AbilityType, IAbility> abilities)
        {
            _abilities = abilities;
        }

        public T GetAbility<T>(AbilityType abilityType) where T : IAbility
        {
            var ability = _abilities[abilityType];
            var result = (T)ability;
            return result;
        }

        public class Factory : PlaceholderFactory<ICreatureBalanceModel, CreatureModel> { }
    }
}