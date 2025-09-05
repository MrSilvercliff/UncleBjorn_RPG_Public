using _Project.Scripts.Project.UI.Data;
using _Project.Scripts.Project.UI.Data.Presenters;
using UnityEngine;

namespace _Project.Scripts.Project.UI.Popups.SceneLoading
{
    public interface ISceneLoadingPopupUIData : IUIData
    {
        void SetProgress(float progress);
    }

    public class SceneLoadingPopupUIData : UIData, ISceneLoadingPopupUIData
    {
        [SerializeField] private ProgressBarPresenter _progressBarPresenter;
        
        public override void Init()
        {
            _progressBarPresenter.Init();
            base.Init();
        }

        public void SetProgress(float progress)
        {
            _progressBarPresenter.SetProgress(progress);
        }
    }
}
