using System;
using System.Collections.Generic;

namespace DataStructures.PriorityQueue
{
    public class BinaryHeap<T> where T : IComparable<T>
    {
        private int _currentSize;
        private T[] _array;
        private static readonly int DEFAULT_CAPACITY = 100;

        public BinaryHeap()
        {
            _currentSize = 0;
            _array = new T[DEFAULT_CAPACITY + 1];
        }

        public bool IsEmpty
        {
            get { return _currentSize == 0; }
        }

        public T Min
        {
            get
            {
                if (IsEmpty)
                    throw new Exception();
                return _array[1];
            }
        }

        public int Size { get { return _currentSize;} }

        public bool Add(T x)
        {
            if (_currentSize + 1 == _array.Length)
                doubleArray();
            int hole = ++_currentSize;
            _array[0] = x;
            
            for (; x.CompareTo(_array[hole / 2]) < 0; hole /= 2)
                _array[hole] = _array[hole / 2];
            _array[hole] = x;
            return true;
        }

        private void doubleArray()
        {
            T[] newArray;
            newArray = new T[_array.Length * 2];
            for (int i = 0; i < _array.Length; i++)
                newArray[i] = _array[i];
            _array = newArray;
        }

        //Adds a value to the heap without rebuilding the heap
        public void AddFreely(T x)
        {
            if (_currentSize + 1 == _array.Length)
                doubleArray();
            _array[_currentSize + 1] = x;
            _currentSize++;
        }

        public void Clear()
        {
            _currentSize = 0;
        }

        public T DeleteMin()
        {
            T minItem = Min;
            _array[1] = _array[_currentSize--];
            percolateDown(1);
            return minItem;
        }

        private void percolateDown(int hole)
        {
            int child;
            T tmp = _array[hole];
            for (; hole * 2 <= _currentSize; hole = child)
            {
                child = hole * 2;
                if (child != _currentSize && _array[child + 1].CompareTo(_array[child])<0)
                    child++;
                if (_array[child].Equals(tmp))
                    _array[hole] = _array[child];
                else
                    break;
            }
            _array[hole] = tmp;
        }

        public void BuildHeap()
        {
            for (int i = _currentSize / 2; i > 0; i--)
                percolateDown(i);
        }


    }
}