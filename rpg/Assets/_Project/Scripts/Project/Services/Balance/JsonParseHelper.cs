using Defective.JSON;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using UnityEngine;

namespace _Project.Scripts.Project.Services.Balance
{
    public class JsonParseHelper : JSONParseHelperBase
    {
        protected override T ParseItem<T>(JSONObject itemJson)
        {
            return default(T);
        }
    }
}