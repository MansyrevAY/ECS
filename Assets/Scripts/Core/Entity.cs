namespace Core
{
    public struct Entity
    {
        public readonly ulong Id;
        public ulong ComponentMask { get; private set; }

        public Entity(ulong id)
        {
            Id = id;
            ComponentMask = 0;
        }

        public void AddComponent(IComponent component)
        {
            ComponentMask |= component.Signature;
        }

        public void RemoveComponent(IComponent component)
        {
            ComponentMask &= ~component.Signature;
        }
    }
}