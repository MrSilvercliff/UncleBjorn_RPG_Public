using System;
using System.Collections.Generic;
using Defective.JSON;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse
{
    public interface IJSONParseHelper
    {
        T ParseEnum<T>(JSONObject json, string key, T defaultValue = default, bool ignoreCase = true) where T : struct;
        
        Vector2 ParseVector2(JSONObject json, string key);
        Vector2 ParseVector2(JSONObject json);
        
        Vector2Int ParseVector2Int(JSONObject json, string key);
        Vector2Int ParseVector2Int(JSONObject json);
        
        Vector3 ParseVector3(JSONObject json, string key);
        Vector3 ParseVector3(JSONObject json);
        
        Vector3Int ParseVector3Int(JSONObject json, string key);
        Vector3Int ParseVector3Int(JSONObject json);
        
        List<T> ParseList<T>(JSONObject json, string key);
    }

    public abstract class JSONParseHelperBase : IJSONParseHelper
    {
        private Dictionary<Type, Func<JSONObject, int, object>> _defaultParseFunctions;

        public JSONParseHelperBase()
        {
            _defaultParseFunctions = new Dictionary<Type, Func<JSONObject, int, object>>();
            _defaultParseFunctions[typeof(string)] = ParseString;
            _defaultParseFunctions[typeof(int)] = ParseInt;
            _defaultParseFunctions[typeof(float)] = ParseFloat;
            _defaultParseFunctions[typeof(Vector2)] = ParseVector2;
            _defaultParseFunctions[typeof(Vector2Int)] = ParseVector2Int;
            _defaultParseFunctions[typeof(Vector3)] = ParseVector3;
            _defaultParseFunctions[typeof(Vector3Int)] = ParseVector3Int;
        }

        public T ParseEnum<T>(JSONObject json, string key, T defaultValue = default, bool ignoreCase = true)
            where T : struct
        {
            var enumStringValue = json[key].stringValue;

            var tryParseResult = Enum.TryParse<T>(enumStringValue, ignoreCase, out T result);

            if (tryParseResult)
                return result;

            LogUtils.Error(typeof(JSONParseHelperBase), $"Cant parse enum value [{enumStringValue}] to type [{typeof(T).Name}]");
            return defaultValue;
        }

        #region Vector2

        public Vector2 ParseVector2(JSONObject json, string key)
        {
            var vectorJson = json[key];
            var result = ParseVector2(vectorJson);
            return result;
        }

        public Vector2 ParseVector2(JSONObject json)
        {
            var x = json["x"].floatValue;
            var y = json["y"].floatValue;
            var result = new Vector2(x, y);
            return result;
        }

        #endregion Vector2


        #region Vector2Int

        public Vector2Int ParseVector2Int(JSONObject json, string key)
        {
            var vectorJson = json[key];
            var result = ParseVector2Int(json);
            return result;
        }

        public Vector2Int ParseVector2Int(JSONObject json)
        {
            var x = json["x"].intValue;
            var y = json["y"].intValue;
            var result = new Vector2Int(x, y);
            return result;
        }

        #endregion Vector2Int


        #region Vector3

        public Vector3 ParseVector3(JSONObject json, string key)
        {
            var vectorJson = json[key];
            var result = ParseVector3(vectorJson);
            return result;
        }

        public Vector3 ParseVector3(JSONObject json)
        {
            var x = json["x"].floatValue;
            var y = json["y"].floatValue;
            var z = json["z"].floatValue;
            var result = new Vector3(x, y, z);
            return result;
        }

        #endregion Vector3


        #region Vector3Int

        public Vector3Int ParseVector3Int(JSONObject json, string key)
        {
            var vectorJson = json[key];
            var result = ParseVector3Int(vectorJson);
            return result;
        }

        public Vector3Int ParseVector3Int(JSONObject json)
        {
            var x = json["x"].intValue;
            var y = json["y"].intValue;
            var z = json["z"].intValue;
            var result = new Vector3Int(x, y, z);
            return result;
        }

        #endregion


        public List<T> ParseList<T>(JSONObject json, string key)
        {
            var array = json[key];
            var typeT = typeof(T);
            object parsedItem = null;

            var result = new List<T>();

            for (int i = 0; i < array.list.Count; i++)
            {
                if (_defaultParseFunctions.TryGetValue(typeT, out var func))
                    parsedItem = func.Invoke(array, i);
                else
                {
                    var itemJson = array[i];
                    parsedItem = ParseItem<T>(itemJson);
                }

                result.Add((T)parsedItem);
            }

            return result;
        }

        #region ParseListPrimitives

        private object ParseString(JSONObject array, int index)
        {
            var result = array[index].stringValue;
            return result;
        }

        private object ParseInt(JSONObject array, int index)
        {
            var result = array[index].intValue;
            return result;
        }

        private object ParseFloat(JSONObject array, int index)
        {
            var result = array[index].floatValue;
            return result;
        }

        private object ParseVector2(JSONObject array, int index)
        {
            var itemJson = array[index];
            var result = ParseVector2(itemJson);
            return result;
        }

        private object ParseVector2Int(JSONObject array, int index)
        {
            var itemJson = array[index];
            var result = ParseVector2Int(itemJson);
            return result;
        }

        private object ParseVector3(JSONObject array, int index)
        {
            var itemJson = array[index];
            var result = ParseVector3(itemJson);
            return result;
        }

        private object ParseVector3Int(JSONObject array, int index)
        {
            var itemJson = array[index];
            var result = ParseVector3Int(itemJson);
            return result;
        }

        #endregion ParseListPrimitives

        protected abstract T ParseItem<T>(JSONObject itemJson);
    }
}
