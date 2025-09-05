using _Project.Scripts.GameScene.Abilities;
using _Project.Scripts.GameScene.Creatures.Basis;
using _Project.Scripts.GameScene.Services.Abilities;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.Balance;
using _Project.Scripts.Project.Services.Balance.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.GameScene.Services.Creatures
{
    public interface ICreatureModelCreator
    {
        ICreatureModel GetCreatureModel(string creatureId);
    }

    public class CreatureModelCreator : ICreatureModelCreator
    {
        [Inject] private IProjectBalanceStorage _projectBalanceStorage;
        [Inject] private CreatureModel.Factory _creatureModelFactory;
        [Inject] private IAbilitiesProvider _abilitiesProvider;

        public ICreatureModel GetCreatureModel(string creatureId)
        {
            var creatureBalanceStorage = _projectBalanceStorage.Creatures;

            var tryGetResult = creatureBalanceStorage.TryGetById(creatureId, out var balanceModel);

            if (!tryGetResult)
            {
                LogUtils.Error(this, $"Creature balance model with id [{creatureId}] does not exist!");
                return null;
            }

            var creatureModel = _creatureModelFactory.Create(balanceModel);
            var abilities = GetAbilities(balanceModel);
            creatureModel.Setup(abilities);
            return creatureModel;
        }

        private IReadOnlyDictionary<AbilityType, IAbility> GetAbilities(ICreatureBalanceModel balanceModel)
        {
            var result = new Dictionary<AbilityType, IAbility>();

            var abilityIds = balanceModel.Abilities;

            foreach (var abilityId in abilityIds)
            {
                var ability = _abilitiesProvider.GetAbility(abilityId);
                result[ability.AbilityType] = ability;
            }

            return result;
        }
    }
}