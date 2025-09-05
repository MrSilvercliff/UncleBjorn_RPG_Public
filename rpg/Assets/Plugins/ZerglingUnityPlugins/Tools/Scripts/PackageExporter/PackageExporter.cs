#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace ZerglingUnityPlugins.Tools.Scripts.PackageExporter
{
    public static class PackageExporter
    {
        private static List<string> _excludeDirectories = new List<string>() { "_Example" };

        public static void Export(string packageName, string assetsPath)
        {
            var path = Path.GetFullPath(assetsPath);
            path = path.Substring(0, path.Length - assetsPath.Length - 1);
            path = $"{path}/Assets/Plugins/ZerglingUnityPlugins/{assetsPath}";

            var versionFilePath = $"Assets/Plugins/ZerglingUnityPlugins/{assetsPath}/version.txt";
            var version = File.ReadAllLines(versionFilePath);
            var name = $"{packageName}-{version[0]}.unitypackage";

            var assetsPathList = new List<string>();

            DirectoryInfo info = new DirectoryInfo(path);
            var directories = info.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                var dir = directories[i];
                if (_excludeDirectories.Contains(dir.Name))
                    continue;

                var directoryPath = $"Assets/Plugins/ZerglingUnityPlugins/{assetsPath}/{dir.Name}";
                assetsPathList.Add(directoryPath);
            }

            assetsPathList.Add(versionFilePath);

            AssetDatabase.ExportPackage(assetsPathList.ToArray(), $"../{name}", ExportPackageOptions.Recurse);
            LogUtils.Info($"{name}", "EXPORT FINISHED");
        }
    }
}
#endif
