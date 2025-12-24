using Core;

namespace Tests
{
    public class TestFactory
    {
        public EntityModule CreateEntityModule()
        {
            return new EntityModule(1000, 1000, 1000);
        }
    }
}