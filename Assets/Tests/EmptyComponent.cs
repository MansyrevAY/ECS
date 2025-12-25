using Core;

namespace Tests
{
    public struct EmptyComponent : IComponent
    {
        public ulong Signature { get; private set; }

        public ulong Id { get; set; }
            
        public void SetSignature(ulong signature)
        {
            Signature = signature;
        }
    }
}