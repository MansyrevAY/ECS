namespace Core
{
    public interface IComponentProvider
    {
        T GetComponent<T>(ref Entity entity) where T : struct, IComponent;
    }
}