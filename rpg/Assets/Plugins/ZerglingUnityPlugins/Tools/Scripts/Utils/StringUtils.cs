using System;

namespace ZerglingUnityPlugins.Tools.Scripts.Utils
{
    public static class StringUtils
    {
        public static T ParseEnum<T>(string value, T defaultType, bool ignoreCase = true) where T : struct
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch
            {
                return defaultType;
            }
        }
    }
}
