using System.Text;
using Defective.JSON;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using UnityEngine;

namespace _Project.Scripts.Project.Services.Balance.Models
{
    public interface IInteractableObjectsBalanceModel : IBalanceModelBase
    {
        float InteractMaxRange { get; }
        float PlayerRotateLookInteractBlockTreshold { get; }
        float DoorInteractTimeSeconds { get; }
    }

    public class InteractableObjectsBalanceModel : BalanceModelBase, IInteractableObjectsBalanceModel
    {
        public float InteractMaxRange => _interactMaxRange;
        public float PlayerRotateLookInteractBlockTreshold => _playerRotateLookInteractBlockTreshold;
        public float DoorInteractTimeSeconds => _doorInteractTimeSeconds;

        private float _interactMaxRange;
        private float _playerRotateLookInteractBlockTreshold;
        private float _doorInteractTimeSeconds;
        
        protected override void OnTrySetup(JSONObject json, IJSONParseHelper parseHelper)
        {
            _interactMaxRange = json["interact_max_range"].floatValue;
            _playerRotateLookInteractBlockTreshold = json["player_rotate_look_interact_block_treshold"].floatValue;
            _doorInteractTimeSeconds = json["door_interact_time_seconds"].floatValue;
        }

        public override void DebugPrint()
        {
            var builder = new StringBuilder();
            
            builder.AppendLine($"_interactMaxRange = {_interactMaxRange}");
            builder.AppendLine($"_playerRotateLookInteractBlockTreshold = {_playerRotateLookInteractBlockTreshold}");
            builder.AppendLine($"_doorInteractTimeSeconds = {_doorInteractTimeSeconds}");
            
            Debug.Log(builder.ToString());
        }
    }
}
