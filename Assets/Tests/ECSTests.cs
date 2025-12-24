using System.Collections.Generic;
using Core;
using NUnit.Framework;

namespace Tests
{
    public class ECSTests
    {
        private TestFactory _factory;
        
        [OneTimeSetUp]
        public void Entity_Module_Constructor_Test()
        {
            _factory = new TestFactory();
        }

        [Test]
        public void Create_Entity_Test()
        {
            var module = _factory.CreateEntityModule();
            var entity = module.CreateEntity();
            
            Assert.IsNotNull(entity);
        }

        [Test]
        public void Entities_Are_Different_Test()
        {
            var module = _factory.CreateEntityModule();
            var entities = new List<Entity>();

            for (int i = 0; i < 100; i++)
            {
                entities.Add(module.CreateEntity());
            }
            
            Assert.That(entities, Is.Unique);
        }

        [Test]
        public void Entity_Id_Is_Reused_Test()
        {
            var module = _factory.CreateEntityModule();
            var entity = module.CreateEntity();
            var id = entity.Id;
            
            module.ReleaseEntity(ref entity);
            
            var newEntity = module.CreateEntity();
            
            Assert.That(newEntity.Id, Is.EqualTo(id));
        }

        [Test]
        public void Component_Type_Is_Registered_Test()
        {
            var module = _factory.CreateEntityModule();
            module.RegisterComponent<TestComponent>();
        }

        [Test]
        public void HasComponent_True_Positive_Test()
        {
            var module = _factory.CreateEntityModule();
            var entity = module.CreateEntity();
            module.RegisterComponent<TestComponent>();
            
            module.AddComponent<TestComponent>(ref entity, out _);
            
            Assert.IsTrue(module.HasComponent<TestComponent>(ref entity));
        }

        [Test]
        public void HasComponent_True_Negative_Test()
        {
            var module = _factory.CreateEntityModule();
            var entity = module.CreateEntity();
            module.RegisterComponent<TestComponent>();
            
            Assert.IsFalse(module.HasComponent<TestComponent>(ref entity));
        }

        [Test]
        public void RemoveComponent_Removes_Properly_Test()
        {
            var module = _factory.CreateEntityModule();
            var entity = module.CreateEntity();
            module.RegisterComponent<TestComponent>();
            
            module.AddComponent<TestComponent>(ref entity, out var id);
            
            module.RemoveComponent(ref entity, id);
            
            Assert.IsFalse(module.HasComponent<TestComponent>(ref entity));
        }
    }
}
