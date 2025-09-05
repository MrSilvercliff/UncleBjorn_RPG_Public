using _Project.Scripts.Project.Enums;
using System.Text;
using Defective.JSON;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using UnityEngine;

namespace _Project.Scripts.Project.Services.Balance.Models
{
    public interface IAbilityBalanceModel : IBalanceModelWithIdBase
    {
        AbilityType AbilityType { get; }
        int RequiredLevel { get; }
        string NameKey { get; }
        string DescriptionKey { get; }

        int IntValue1 { get; }
        int IntValue2 { get; }
        int IntValue3 { get; }
        int IntValue4 { get; }

        float FloatValue1 { get; }
        float FloatValue2 { get; }
        float FloatValue3 { get; }
        float FloatValue4 { get; }

        bool BoolValue1 { get; }
        bool BoolValue2 { get; }

        string StringValue1 { get; }
        string StringValue2 { get; }
    }

    public class AbilityBalanceModel : BalanceModelWithIdBase, IAbilityBalanceModel
    {
        public AbilityType AbilityType => _abilityType;
        public int RequiredLevel => _requiredLevel;
        public string NameKey => _nameKey;
        public string DescriptionKey => _descriptionKey;

        public int IntValue1 => _intValue1;
        public int IntValue2 => _intValue2;
        public int IntValue3 => _intValue3;
        public int IntValue4 => _intValue4;

        public float FloatValue1 => _floatValue1;
        public float FloatValue2 => _floatValue2;
        public float FloatValue3 => _floatValue3;
        public float FloatValue4 => _floatValue4;

        public bool BoolValue1 => _boolValue1;
        public bool BoolValue2 => _boolValue2;

        public string StringValue1 => _stringValue1;
        public string StringValue2 => _stringValue2;

        private AbilityType _abilityType;
        private int _requiredLevel;
        private string _nameKey;
        private string _descriptionKey;

        private int _intValue1;
        private int _intValue2;
        private int _intValue3;
        private int _intValue4;

        private float _floatValue1;
        private float _floatValue2;
        private float _floatValue3;
        private float _floatValue4;

        private bool _boolValue1;
        private bool _boolValue2;

        private string _stringValue1;
        private string _stringValue2;
        
        protected override void OnTrySetup(JSONObject json, IJSONParseHelper parseHelper)
        {
            _id = json["id"].stringValue;
            _abilityType = parseHelper.ParseEnum(json, "type", AbilityType.NONE);
            _requiredLevel = json["required_level"].intValue;
            _nameKey = json["name_key"].stringValue;
            _descriptionKey = json["description_key"].stringValue;
            
            _intValue1 = json["int_value_1"].intValue;
            _intValue2 = json["int_value_2"].intValue;
            _intValue3 = json["int_value_3"].intValue;
            _intValue4 = json["int_value_4"].intValue;
            
            _floatValue1 = json["float_value_1"].floatValue;
            _floatValue2 = json["float_value_2"].floatValue;
            _floatValue3 = json["float_value_3"].floatValue;
            _floatValue4 = json["float_value_4"].floatValue;

            _boolValue1 = json["bool_value_1"].boolValue;
            _boolValue2 = json["bool_value_2"].boolValue;
            
            _stringValue1 = json["string_value_1"].stringValue;
            _stringValue2 = json["string_value_2"].stringValue;
        }

        public override void DebugPrint()
        {
            var builder = new StringBuilder();
            
            builder.AppendLine($"_abilityType = {_abilityType}");
            builder.AppendLine($"_requiredLevel = {_requiredLevel}");
            builder.AppendLine($"_nameKey = {_nameKey}");
            builder.AppendLine($"_descriptionKey = {_descriptionKey}");

            builder.AppendLine($"_intValue1 = {_intValue1}");
            builder.AppendLine($"_intValue2 = {_intValue2}");
            builder.AppendLine($"_intValue3 = {_intValue3}");
            builder.AppendLine($"_intValue4 = {_intValue4}");

            builder.AppendLine($"_floatValue1 = {_floatValue1}");
            builder.AppendLine($"_floatValue2 = {_floatValue2}");
            builder.AppendLine($"_floatValue3 = {_floatValue3}");
            builder.AppendLine($"_floatValue4 = {_floatValue4}");

            builder.AppendLine($"_boolValue1 = {_boolValue1}");
            builder.AppendLine($"_boolValue2 = {_boolValue2}");

            builder.AppendLine($"_stringValue1 = {_stringValue1}");
            builder.AppendLine($"_stringValue2 = {_stringValue2}");
            
            Debug.Log(builder.ToString());
        }
    }
}