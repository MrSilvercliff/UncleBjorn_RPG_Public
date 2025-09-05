using _Project.Scripts.GameScene.Creatures.Basis.Components;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.GameScene.Creatures.Basis
{
    public interface ICreatureComponentContainer : IFlushable, IMonoFixedUpdatable, IMonoUpdatable, IMonoLateUpdatable
    {
        void InitComponents(ICreatureController creatureController, IReadOnlyCollection<ICreatureComponentBase> components);
        T GetComponent<T>(CreatureComponentType componentType) where T : ICreatureComponentBase;

        void SubscribeFixedUpdatable(ICreatureComponentFixedUpdatableBase component);
        void SubscribeUpdatable(ICreatureComponentUpdatable component);
        void SubscribeLateUpdatable(ICreatureComponentLateUpdatable component);
    }

    public class CreatureComponentContainer : ICreatureComponentContainer
    {
        private readonly Dictionary<CreatureComponentType, ICreatureComponentBase> _allComponents;
        private readonly HashSet<ICreatureComponentFixedUpdatableBase> _fixedUpdatableComponents;
        private readonly HashSet<ICreatureComponentUpdatable> _updatableComponents;
        private readonly HashSet<ICreatureComponentLateUpdatable> _lateUpdatableComponents;

        public CreatureComponentContainer()
        {
            _allComponents = new();
            _fixedUpdatableComponents = new();
            _updatableComponents = new();
            _lateUpdatableComponents = new();
        }

        public void InitComponents(ICreatureController creatureController, IReadOnlyCollection<ICreatureComponentBase> components)
        {
            foreach (var component in components) 
            {
                var componentType = component.ComponentType;
                _allComponents[componentType] = component;
                component.Init(this, creatureController);
            }
        }

        public bool Flush()
        {
            _allComponents.Clear();
            _fixedUpdatableComponents.Clear();
            _updatableComponents.Clear();
            _lateUpdatableComponents.Clear();
            return true;
        }

        public T GetComponent<T>(CreatureComponentType componentType) where T : ICreatureComponentBase
        {
            var result = (T)_allComponents[componentType];
            return result;
        }

        public void SubscribeFixedUpdatable(ICreatureComponentFixedUpdatableBase component)
        {
            _fixedUpdatableComponents.Add(component);
        }

        public void SubscribeUpdatable(ICreatureComponentUpdatable component)
        {
            _updatableComponents.Add(component);
        }

        public void SubscribeLateUpdatable(ICreatureComponentLateUpdatable component)
        {
            _lateUpdatableComponents.Add(component);
        }

        public void OnFixedUpdate()
        {
            foreach (var component in _fixedUpdatableComponents)
                component.OnFixedUpdate();
        }

        public void OnUpdate()
        {
            foreach (var component in _updatableComponents)
                component.OnUpdate();
        }

        public void OnLateUpdate()
        {
            foreach (var component in _lateUpdatableComponents)
                component.OnLateUpdate();
        }
    }
}