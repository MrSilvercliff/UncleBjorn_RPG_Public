using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public class SingleGameObjectPool
    {
        private readonly Stack<GameObject> _stack = new Stack<GameObject>();
        private GameObject _source;

        public int Count => _stack.Count;

        public void SetSourceObject(GameObject item)
        {
            _source = item;
        }

        public void Push(GameObject item)
        {
            _stack.Push(item);
            item.gameObject.SetActive(false);
        }

        public GameObject Get(Transform parent = null, bool setActiveGameObject = true)
        {
            if (_source == null)
            {
                Debug.LogError($"[{GetType().Name}] No source object!");
                return null;
            }

            GameObject res = _stack.Count > 0 ? _stack.Pop() : Object.Instantiate(_source, parent == null ? _source.transform.parent : parent);

            if (res == null)
            {
                Debug.LogError($"[{GetType().Name}] Result is null! source: {_source.name}", parent == null ? _source.transform.parent.gameObject : parent.gameObject);
                return null;
            }

            if (setActiveGameObject)
                res.gameObject.SetActive(true);

            return res;
        }

        public void Clear()
        {
            foreach (GameObject obj in _stack)
                Object.Destroy(obj);

            _stack.Clear();
        }
    }
}