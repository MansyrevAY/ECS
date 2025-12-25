using System;

namespace Core.ComponentPool
{
    public sealed class ComponentPool<T> : IComponentPool where T: struct
    {
        public Type Type { get; } = typeof(T);
        public T[] Items;
        public readonly bool[] Has;
        
        public ComponentPool(int size)
        {
            Items = new T[size];
            Has = new bool[size];
        }
    }
}