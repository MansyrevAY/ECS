namespace Core
{
    public record Component
    {
        public readonly ulong Id;

        public Component(ulong id)
        {
            Id = id;
        }
    }
}