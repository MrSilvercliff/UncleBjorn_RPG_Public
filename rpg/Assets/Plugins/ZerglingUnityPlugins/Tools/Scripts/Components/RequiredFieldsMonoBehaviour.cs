using System.Reflection;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.Tools.Scripts.Utils;
using UnityEngine;

namespace ZerglingUnityPlugins.Tools.Scripts.Components
{
    public class RequiredFieldsMonoBehaviour : MonoBehaviour
    {
        protected virtual void OnValidate()
        {
            var type = GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                {
                    var value = field.GetValue(this);
                    if (value == null || value.Equals(null)) // for fake null of Unity object
                    {
                        var hierarchy = transform.GetPath();
                        var message = $"Field <b>{field.Name}</b> not set in component <b>{type.Name}</b> with hierarchy <b>{hierarchy}</b>";
                        LogUtils.Error(this, message);
                    }
                }
            }
        }
    }
}
