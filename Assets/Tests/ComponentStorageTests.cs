using Core;
using NUnit.Framework;

namespace Tests
{
    public class ComponentStorageTests
    {
        private TestFactory _factory;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new TestFactory();
        }
        
        [Test]
        public void Add_Component_Test()
        {
            var storage = _factory.CreateComponentStorage();
            var entity = new Entity(0);
            var component = new EmptyComponent();
            
            storage.Add(component, ref entity);
        }

        [Test]
        public void Has_Component_True_Positive_Test()
        {
            var storage = _factory.CreateComponentStorage();
            var entity = new Entity(0);
            var component = new EmptyComponent();
            
            storage.Add(component, ref entity);

            var hasComponent = storage.Has<EmptyComponent>(ref entity);
            
            Assert.IsTrue(hasComponent);
        }

        [Test]
        public void Has_Component_Not_Registered_Test()
        {
            var storage = _factory.CreateComponentStorage();
            var entity = new Entity(0);

            var hasComponent = storage.Has<EmptyComponent>(ref entity);
            
            Assert.IsFalse(hasComponent);
        }
        
        [Test]
        public void Has_Component_True_Negative_Test()
        {
            var storage = _factory.CreateComponentStorage();
            var entity = new Entity(0);
            var component = new EmptyComponent();
            
            storage.Add(component, ref entity);

            var hasComponent = storage.Has<IntComponent>(ref entity);
            
            Assert.IsFalse(hasComponent);
        }

        [Test]
        public void Get_Component_Test()
        {
            var storage = _factory.CreateComponentStorage();
            var entity = new Entity(0);
            var component = new EmptyComponent();
            
            storage.Add(component, ref entity);
            
            var receivedComponent = storage.Get<EmptyComponent>(ref entity);
            
            Assert.AreEqual(component, receivedComponent);
        }
    }
}