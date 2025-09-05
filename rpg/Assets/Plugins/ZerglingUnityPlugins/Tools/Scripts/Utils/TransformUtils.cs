using UnityEngine;

namespace ZerglingUnityPlugins.Tools.Scripts.Utils
{
    public static class TransformUtils
    {
        public static string GetPath(this Transform current)
        {
            if (current == null)
                return "null";

            if (current.parent == null)
                return current.name;

            return current.parent.GetPath() + "/" + current.name;
        }
    }
}
