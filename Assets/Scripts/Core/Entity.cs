namespace Core
{
    public struct Entity
    {
        public readonly ulong Id;
        public ulong ComponentMask;

        public Entity(ulong id)
        {
            Id = id;
            ComponentMask = 0;
        }

        public void AddComponent(Component component)
        {
            ComponentMask |= component.Id;
        }

        public void RemoveComponent(Entity component)
        {
            ComponentMask &= ~component.Id;
        }
    }
}