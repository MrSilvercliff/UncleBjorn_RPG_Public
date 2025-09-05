using _Project.Scripts.GameScene.Abilities;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.Balance;
using _Project.Scripts.Project.Services.Balance.Models;
using _Project.Scripts.Project.Services.Balance.Storages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.GameScene.Services.Abilities
{
    public interface IAbilitiesProvider
    {
        IAbility GetAbility(string id);
    }

    public class AbilitiesProvider : IAbilitiesProvider
    {
        [Inject] private IProjectBalanceStorage _projectBalanceStorage;

        [Inject] private AbilityBasicMove.Factory _moveFactory;

        private Dictionary<string, IAbility> _abilitiesById;

        public AbilitiesProvider()
        {
            _abilitiesById = new();
        }

        public IAbility GetAbility(string id)
        {
            var tryGetResult = _abilitiesById.TryGetValue(id, out var existAbility);

            if (tryGetResult)
                return existAbility;

            var abilitiesBalanceStorage = _projectBalanceStorage.Abilities;
            tryGetResult = abilitiesBalanceStorage.TryGetById(id, out var balanceModel);

            if (!tryGetResult)
            {
                LogUtils.Error(this, $"Ability balance model with id [{id}] does not exist!");
                return null;
            }

            var newAbility = CreateAbility(balanceModel);
            _abilitiesById[id] = newAbility;
            return newAbility;
        }

        private IAbility CreateAbility(IAbilityBalanceModel balanceModel)
        {
            var abilityType = balanceModel.AbilityType;

            IAbility result = null;

            switch (abilityType)
            {
                case AbilityType.BASIC_MOVE:
                    result = _moveFactory.Create(balanceModel);
                    break;

                default:
                    LogUtils.Error(this, $"Ability factory for ability type [{abilityType}] does not implemented!");
                    break;
            }

            return result;
        }
    }
}