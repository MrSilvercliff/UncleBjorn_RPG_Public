using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZerglingUnityPlugins.ZenjectExtentions.ContextProvider
{
    public interface IZenjectContextProvider
    {
        DiContainer Container { get; }

        void UpdateContext(SceneContext context);
    }

    public class ZenjectContextProvider : IZenjectContextProvider
    {
        public DiContainer Container { get; private set; }

        public void UpdateContext(SceneContext context)
        {
            if (context == null)
            {
                Container = ProjectContext.Instance.Container;
                return;
            }

            Container = context.Container;
        }
    }
}
