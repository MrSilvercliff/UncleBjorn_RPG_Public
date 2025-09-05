using UnityEngine;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public class GameObjectByNamePool : Pool<string, Object>
    {
        protected override Object Instantiate(Object obj)
        {
            return Object.Instantiate(obj);
        }

        public override void Push(Object obj)
        {
            Push(obj.name, obj);
        }

        protected override string GetKey(Object obj)
        {
            return obj.name;
        }
    }
}

