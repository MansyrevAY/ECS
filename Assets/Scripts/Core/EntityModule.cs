using System.Collections.Generic;

namespace Core
{
    public class EntityModule
    {
        private readonly Entity[] _entities;
        private readonly Component[] _components;
        
        private readonly Stack<ulong> _freeEntities = new();

        public EntityModule(int maxEntities, int maxComponents)
        {
            _entities = new Entity[maxEntities];

            ulong freeId = 0;
            for (var i = 0; i < _entities.Length; i++)
            {
                _freeEntities.Push(freeId);
                freeId++;
            }
            
            _components = new Component[maxComponents];
        }

        public Entity CreateEntity()
        {
            var nextIndex = _freeEntities.Pop();
            _entities[nextIndex] = new Entity(nextIndex);
            
            return _entities[nextIndex];
        }

        public void ReleaseEntity(Entity entity)
        {
            _freeEntities.Push(entity.Id);
        }
    }
}