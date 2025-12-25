using Core;
using Core.ComponentPool;

namespace Tests
{
    public class TestFactory
    {
        public EntityModule CreateEntityModule()
        {
            return new EntityModule(1000, 1000, 1000);
        }

        public ComponentStorage CreateComponentStorage()
        {
            return new ComponentStorage(20);
        }
    }
}