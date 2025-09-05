using _Project.Scripts.Project.Enums;
using System.Collections.Generic;
using System.Text;
using Defective.JSON;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using UnityEngine;

namespace _Project.Scripts.Project.Services.Balance.Models
{
    public interface ICreatureBalanceModel : IBalanceModelWithIdBase
    { 
        CreatureType CreatureType { get; }
        string NameKey { get; }
        string DescriptionKey { get; }
        string PrefabId { get; }
        StateMachineType StateMachineType { get; }
        IReadOnlyList<string> Abilities { get; }
    }

    public class CreatureBalanceModel : BalanceModelWithIdBase, ICreatureBalanceModel
    {
        public CreatureType CreatureType => _creatureType;
        public string NameKey => _nameKey;
        public string DescriptionKey => _descriptionKey;
        public string PrefabId => _prefabId;
        public StateMachineType StateMachineType => _stateMachineType;
        public IReadOnlyList<string> Abilities => _abilities;

        private CreatureType _creatureType;
        private string _nameKey;
        private string _descriptionKey;
        private string _prefabId;
        private StateMachineType _stateMachineType;
        private List<string> _abilities;
        
        protected override void OnTrySetup(JSONObject json, IJSONParseHelper parseHelper)
        {
            _id = json["id"].stringValue;
            _creatureType = parseHelper.ParseEnum(json, "type", CreatureType.NONE);
            _nameKey = json["name_key"].stringValue;
            _descriptionKey = json["description_key"].stringValue;
            _prefabId = json["prefab_id"].stringValue;
            _stateMachineType = parseHelper.ParseEnum(json, "state_machine_type", StateMachineType.NONE);
            _abilities = parseHelper.ParseList<string>(json, "abilities");
        }

        public override void DebugPrint()
        {
            var builder = new StringBuilder();
            
            builder.AppendLine($"Id = {Id}");
            builder.AppendLine($"_creatureType = {_creatureType}");
            builder.AppendLine($"_nameKey = {_nameKey}");
            builder.AppendLine($"_descriptionKey = {_descriptionKey}");
            builder.AppendLine($"_prefabId = {_prefabId}");
            builder.AppendLine($"_stateMachineType = {_stateMachineType}");

            builder.AppendLine($"Abilities:");
            for (int i = 0; i < _abilities.Count; i++)
                builder.AppendLine($"_abilities[{i}] = {_abilities[i]}");
            
            Debug.LogError(builder.ToString());
        }
    }
}