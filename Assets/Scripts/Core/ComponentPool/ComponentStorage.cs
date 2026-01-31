using System;
using System.Collections.Generic;

namespace Core.ComponentPool
{
    public class ComponentStorage
    {
        private readonly Dictionary<Type, IComponentPool> _componentPools = new();

        private readonly int _maxEntityAmount;
        
        public ComponentStorage(int maxEntityAmount)
        {
            _maxEntityAmount = maxEntityAmount;
        }

        public void Add<T>(T component, ref Entity entity) where T : struct, IComponent
        {
            if (!_componentPools.ContainsKey(typeof(T)))
            {
                _componentPools.Add(typeof(T), new ComponentPool<T>(_maxEntityAmount));
            }
            
            var pool = _componentPools[typeof(T)] as ComponentPool<T>;
            
            pool.Items[entity.Id] = component;
            pool.Has[entity.Id] = true;
        }

        public bool Has<T>(ref Entity entity) where T : struct, IComponent
        {
            if (!_componentPools.ContainsKey(typeof(T)))
            {
                return false;
            }

            var pool = _componentPools[typeof(T)] as ComponentPool<T>;
            return pool.Has[entity.Id];
        }

        public ref T Get<T>(ref Entity entity) where T : struct, IComponent
        {
            if (!_componentPools.ContainsKey(typeof(T)))
            {
                throw new ArgumentOutOfRangeException("T", $"Component of type {typeof(T)} is not registered");
            }
            
            var pool = _componentPools[typeof(T)] as ComponentPool<T>;

            if (!pool.Has[entity.Id])
            {
                throw new InvalidOperationException($"Pool does not contain component for entity {entity.Id}");
            }
            
            return ref pool.Items[entity.Id];
        }

        public void Remove<T>(ref Entity entity) where T : struct, IComponent
        {
            if (!Has<T>(ref entity))
            {
                return;
            }
            
            var pool = _componentPools[typeof(T)] as ComponentPool<T>;
            
            pool.Has[entity.Id] = false;
        }
    }
}