using UnityEngine;

namespace _Project.Scripts.Project.UI.Data
{
    public interface IUIPresenter
    {
        void Init();
    }

    public class UIPresenter : MonoBehaviour, IUIPresenter
    {
        public virtual void Init()
        {
        }
    }
}
