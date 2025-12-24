using System.Collections.Generic;

namespace Core
{
    public interface ISystem
    {
        ulong ComponentMask { get; }
        void AddComponent(ulong componentId);
        void RemoveComponent(ulong componentId);
        void Update(ref Entity entity, IComponentProvider provider, IEnumerable<ulong> components);
    }
}