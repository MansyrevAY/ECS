using System;
using System.Collections;
using System.Collections.Generic;

namespace Core
{
    public class ContinuousArray<T> : IEnumerable<T>
    {
        private readonly T[] _elements;
        
        private int _count;

        public ContinuousArray(int length)
        {
            _elements = new T[length];
            _count = 0;
        }

        public void Add(T element)
        {
            _elements[_count] = element;
            _count++;
        }

        public T Get(int index)
        {
            return _elements[index];
        }

        public void Remove(T element)
        {
            var indexToRemove = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_elements[i].Equals(element))
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove == -1)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            _elements[indexToRemove] = default;

            if (_count > 1)
            {
                _elements[indexToRemove] = _elements[_count - 1];
            }
            
            _count--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _elements[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}