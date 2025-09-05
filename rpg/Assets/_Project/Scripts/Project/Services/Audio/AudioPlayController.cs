using _Project.Scripts.Project.Configs.Audio;
using _Project.Scripts.Project.Enums;
using Assets.Plugins.ZerglingUnityPlugins.Tools.Scripts.Mono;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.Project.Services.Audio
{
    public interface IAudioPlayController : IProjectSerivce, IMonoLateUpdatable
    {
        void Play(AudioClipId clipId);
        void Play(IAudioClipInfo clipInfo);
        void Stop(AudioClipId clipId);
    }

    public class AudioPlayController : IAudioPlayController
    {
        [Inject] private IMonoUpdater _monoUpdater;

        [Inject] private IAudioConfig _config;

        [Inject] private AudioSourceControllerPool _pool;

        private List<IAudioSourceController> _playingControllers;
        private List<IAudioSourceController> _nonPlayingControllers;
        private Dictionary<AudioClipId, List<IAudioSourceController>> _controllersById;

        public Task<bool> Init()
        {
            _config.Init();
            _playingControllers = new List<IAudioSourceController>();
            _nonPlayingControllers = new List<IAudioSourceController>();
            _controllersById = new Dictionary<AudioClipId, List<IAudioSourceController>>();
            _monoUpdater.Subscribe(this);
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        public void Play(AudioClipId clipId)
        {
            var clipInfo = _config.GetById(clipId);

            if (clipInfo == null)
                return;

            Play(clipInfo);
        }

        public void Play(IAudioClipInfo clipInfo)
        {
            var clipId = clipInfo.Id;

            var controller = _pool.Spawn();
            _playingControllers.Add(controller);

            if (!_controllersById.ContainsKey(clipId))
                _controllersById[clipId] = new List<IAudioSourceController>();

            _controllersById[clipId].Add(controller);

            controller.Play(clipInfo);
        }

        public void Stop(AudioClipId clipId)
        {
            if (!_controllersById.ContainsKey(clipId))
                return;

            var list = _controllersById[clipId];
            var controller = list[0];

            list.Remove(controller);
            _playingControllers.Remove(controller);

            controller.Stop();
            _nonPlayingControllers.Add(controller);
        }

        public void OnLateUpdate()
        {
            CheckPlayingControllers();
            ClearNonPlayingControllers();
        }

        private void CheckPlayingControllers()
        {
            foreach (var controller in _playingControllers)
            {
                if (controller.IsPlaying)
                    continue;

                _nonPlayingControllers.Add(controller);
            }
        }

        private void ClearNonPlayingControllers()
        {
            foreach (var controller in _nonPlayingControllers)
            {
                var clipId = controller.PlayingClipId;

                var list = _controllersById[clipId];
                list.Remove(controller);
                _playingControllers.Remove(controller);

                _pool.Despawn((AudioSourceController)controller);
            }

            _nonPlayingControllers.Clear();
        }
    }
}