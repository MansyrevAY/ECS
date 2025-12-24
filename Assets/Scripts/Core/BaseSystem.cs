using System.Collections.Generic;

namespace Core
{
    public abstract class BaseSystem : ISystem
    {
        public ulong ComponentMask { get; private set; }
        public void AddComponent(ulong componentId)
        {
            ComponentMask |= componentId;
        }

        public void RemoveComponent(ulong componentId)
        {
            ComponentMask &= ~componentId;
        }

        public abstract void Update(ref Entity entity, IComponentProvider provider, IEnumerable<ulong> components);
    }
}