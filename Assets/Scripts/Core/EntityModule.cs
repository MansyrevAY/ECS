using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Core
{
    public class EntityModule : IComponentProvider
    {
        private readonly Entity[] _entities;
        private readonly Stack<ulong> _freeEntities = new();
        private readonly Stack<ulong> _freeComponents = new();
        private readonly Stack<ulong> _freeComponentSignatures = new();
        
        private readonly Dictionary<ulong, ContinuousArray<ulong>> _entityToComponentMap;
        private readonly Dictionary<Type, ulong> _registeredSignatures = new();
        private readonly Dictionary<ulong, Type> _registeredComponents = new();
        
        private readonly IComponent[] _components;

        private ulong _nextComponentId;

        private readonly ContinuousArray<ISystem> _systems;

        public EntityModule(int maxEntities, int maxComponentSignatures, int maxSystems)
        {
            _entities = new Entity[maxEntities];
            _entityToComponentMap = new Dictionary<ulong, ContinuousArray<ulong>>();
            var maxComponents = maxEntities * maxComponentSignatures;
            _systems = new ContinuousArray<ISystem>(maxSystems);
            
            _components = new IComponent[maxComponents];

            ulong freeEntity = 0;
            for (var i = 0; i < maxEntities; i++)
            {
                _freeEntities.Push(freeEntity);
                freeEntity++;
                _entityToComponentMap.Add(freeEntity, new ContinuousArray<ulong>(maxComponents));
            }
            
            for (var i = 0; i < maxComponentSignatures; i++)
            {
                _freeComponentSignatures.Push(1UL << i);
            }
            
            ulong freeComponents = 0;
            for (var i = 0; i < maxComponents; i++)
            {
                _freeComponents.Push(freeComponents);
                freeComponents++;
            }
        }

        public ref Entity CreateEntity()
        {
            var nextIndex = _freeEntities.Pop();
            _entities[nextIndex] = new Entity(nextIndex);
            
            return ref _entities[nextIndex];
        }

        public void ReleaseEntity(ref Entity entity)
        {
            foreach (var componentId in _entityToComponentMap[entity.Id])
            {
                _freeComponents.Push(componentId);
            }
            
            _entityToComponentMap.Remove(entity.Id);
            _freeEntities.Push(entity.Id);
        }

        public void RegisterComponent<T>() where T : struct, IComponent
        {
            var type = typeof(T);
            
            if (!typeof(IComponent).IsAssignableFrom(type))
            {
                Debug.LogError($"Component type must be subclass of IComponent, was {type.FullName}");
                throw new ArgumentOutOfRangeException();
            }

            _registeredSignatures[type] = _freeComponentSignatures.Pop();
            _registeredComponents[_registeredSignatures[type]] = type;
        }

        public void AddComponent<T>(ref Entity entity, out ulong componentId) where T : struct, IComponent
        {
            componentId = _freeComponents.Pop();
            _components[componentId] = Activator.CreateInstance<T>();
            _components[componentId].SetSignature(_registeredSignatures[typeof(T)]);
            _components[componentId].Id = componentId;
            
            _entityToComponentMap[entity.Id].Add(componentId);
            entity.AddComponent(_components[componentId]);
        }

        public bool HasComponent<T>(ref Entity entity) where T : struct, IComponent
        {
            var componentSignature = _registeredSignatures[typeof(T)];
            
            return (entity.ComponentMask & componentSignature) > 0;
        }

        public void RemoveComponent(ref Entity entity, ulong componentId)
        {
            entity.RemoveComponent(_components[componentId]);
            _components[componentId] = null;
            _entityToComponentMap[entity.Id].Remove(componentId);
            _freeComponents.Push(componentId);
        }

        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
        }

        public void RemoveSystem(ISystem system)
        {
            _systems.Remove(system);
        }

        public void Update()
        {
            foreach (var system in _systems)
            {
                for (var i = 0; i < _entities.Length; i++)
                {
                    var componentCount = X86.Popcnt.popcnt_u64(_entities[i].ComponentMask);
                    var componentIds = new ContinuousArray<ulong>(componentCount);

                    foreach (var componentId in _entityToComponentMap[_entities[i].Id])
                    {
                        if ((_components[componentId].Signature & system.ComponentMask) > 0)
                        {
                            componentIds.Add(componentId);
                        }
                    }

                    system.Update(ref _entities[i], this, componentIds);
                }
            }
        }

        public T GetComponent<T>(ulong id) where T : IComponent
        {
            return (T)_components[id];
        }
    }
}