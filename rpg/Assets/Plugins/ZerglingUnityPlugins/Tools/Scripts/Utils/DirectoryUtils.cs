using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ZerglingUnityPlugins.Tools.Scripts.Utils
{
    public static class DirectoryUtils
    {
        public static string TryCreateDirectory(string path, string name)
        {
            var folderPath = $"{path}/{name}";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return folderPath;
        }
    }
}