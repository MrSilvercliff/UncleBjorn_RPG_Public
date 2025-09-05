using _Project.Scripts.Project.Dotween;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Project.UI.Data.Presenters
{
    public interface IProgressBarPresenter : IUIPresenter
    {
        void SetProgress(float progress);
    }

    public class ProgressBarPresenter : UIPresenter, IProgressBarPresenter
    {
        [SerializeField] private Image _progressFillImage = null;
        [SerializeField] private bool _animationEnabled = false;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private float _duration = 0f;
        [SerializeField] private Ease _ease = Ease.OutQuad; // default Dotween Ease
        
        private float _targetProgress;

        private Sequence _sequence;

        public override void Init()
        {
            _targetProgress = 0f;
            _progressFillImage.fillAmount = 0f;
        }

        public void SetProgress(float progress)
        {
            _targetProgress = progress;
            
            if (_animationEnabled)
                SetProgressWithAnimation();
            else
                SetProgressWithoutAnimation();
        }

        private void SetProgressWithoutAnimation()
        {
            _progressFillImage.fillAmount = _targetProgress;
        }

        private void SetProgressWithAnimation()
        {
            DotweenHelper.KillSequence(_sequence);

            _sequence = DOTween.Sequence();
            _sequence.Append(_progressFillImage.DOFillAmount(_targetProgress, _duration).SetEase(_ease));
            _sequence.SetDelay(_delay);
            _sequence.Play();
        }
    }
}
