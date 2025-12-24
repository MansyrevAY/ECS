namespace Core
{
    public interface IComponent
    {
        ulong Signature { get; }
        ulong Id { get; set; }
        
        void SetSignature(ulong signature);
    }
}