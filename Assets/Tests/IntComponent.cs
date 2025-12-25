using Core;

namespace Tests
{
    public struct IntComponent : IComponent
    {
        public ulong Signature { get; private set; }

        public ulong Id { get; set; }
        
        public int Value { get; set; }
            
        public void SetSignature(ulong signature)
        {
            Signature = signature;
        }
    }
}