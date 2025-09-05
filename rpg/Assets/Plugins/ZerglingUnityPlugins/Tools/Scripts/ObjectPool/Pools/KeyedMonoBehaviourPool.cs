using UnityEngine;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public class KeyedMonoBehaviourPool<TKey, TObject> : ObjectByKeyPool<TKey, TObject> where TObject : MonoBehaviour
    {
        protected override void DestroyObject(TObject obj)
        {
            if (obj != null)
                Object.Destroy(obj.gameObject);
        }
    }
}

