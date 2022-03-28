using System;
using System.Collections.Generic;

namespace HDA.DataStructure
{
    internal class HPriorityQueue<T>
    {
        private HHeap<T> heap;

        public int Count => heap.Count;
        public bool IsEmpty => heap.IsEmpty;

        private Comparer<T> _comparer = Comparer<T>.Default;
        public Comparer<T> Comparer => _comparer;

        public HPriorityQueue()
        {
            heap = new HHeap<T>();
        }

        public HPriorityQueue(Comparer<T> comparer)
        {
            _comparer = comparer;
            heap = new HHeap<T>();
        }

        public void Enqueue(T e)
        {
            heap.Push(e);
        }

        public T Dequeue()
        {
            return heap.Pop();
        }

        public T Peek()
        {
            return heap.Peek();
        }
    }
}
