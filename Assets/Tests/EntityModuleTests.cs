using System;
using System.Collections.Generic;
using Core;
using NUnit.Framework;

namespace Tests
{
    public class EntityModuleTests
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
        public void ReleaseEntity_Releases_Components_Test()
        {
            var module = _factory.CreateEntityModule();
            var entity = module.CreateEntity();
            module.RegisterComponent<EmptyComponent>();
            
            module.AddComponent<EmptyComponent>(ref entity);
            module.ReleaseEntity(ref entity);
            
            Assert.Throws<Exception>(() => module.GetComponent<EmptyComponent>(ref entity));
        }

        [Test]
        public void Component_Type_Is_Registered_Test()
        {
            var module = _factory.CreateEntityModule();
            module.RegisterComponent<EmptyComponent>();
        }

        [Test]
        public void HasComponent_True_Positive_Test()
        {
            var module = _factory.CreateEntityModule();
            var entity = module.CreateEntity();
            module.RegisterComponent<EmptyComponent>();
            
            module.AddComponent<EmptyComponent>(ref entity);
            
            Assert.IsTrue(module.HasComponent<EmptyComponent>(ref entity));
        }

        [Test]
        public void HasComponent_True_Negative_Test()
        {
            var module = _factory.CreateEntityModule();
            var entity = module.CreateEntity();
            module.RegisterComponent<EmptyComponent>();
            
            Assert.IsFalse(module.HasComponent<EmptyComponent>(ref entity));
        }

        [Test]
        public void RemoveComponent_Removes_Properly_Test()
        {
            var module = _factory.CreateEntityModule();
            var entity = module.CreateEntity();
            module.RegisterComponent<EmptyComponent>();
            
            var component = module.AddComponent<EmptyComponent>(ref entity);
            
            module.RemoveComponent(ref entity, ref component);
            
            Assert.IsFalse(module.HasComponent<EmptyComponent>(ref entity));
        }
    }
}
