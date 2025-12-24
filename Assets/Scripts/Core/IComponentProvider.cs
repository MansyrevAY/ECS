namespace Core
{
    public interface IComponentProvider
    {
        T GetComponent<T>(ulong id) where T : IComponent;
    }
}