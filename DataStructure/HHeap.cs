using System;
using System.Collections.Generic;

namespace HDA.DataStructure
{
    // 最大堆
    internal class HHeap<T>
    {
        private HList<T> _data;

        public int Count { get => _data.Count; }
        public bool IsEmpty { get => _data.IsEmpty; }

        private Comparer<T> _comparer = Comparer<T>.Default;
        public Comparer<T> Comparer => _comparer;

        public HHeap(int capacity)
        {
            _data = new HList<T>(capacity);
        }
        public HHeap() : this(10) { }

        public HHeap(int capacity, Comparer<T> comparer)
        {
            _comparer = comparer;
            _data = new HList<T>(capacity);
        }

        public HHeap(T[] array)
        {
            _data = new HList<T>(array);
            for (int i = Parent(Count - 1); i >= 0; i--)
                SiftDown(i);
        }

        public HHeap(T[] array, Comparer<T> comparer)
        {
            _comparer = comparer;
            _data = new HList<T>(array);
            for (int i = Parent(Count - 1); i >= 0; i--)
                SiftDown(i);
        }

        // 返回父亲节点的索引
        private int Parent(int index)
        {
            if (index == 0)
                throw new ArgumentOutOfRangeException("index");
            return (index - 1) / 2;
        }

        // 返回左子节点的索引
        private int LeftChild(int index)
        {
            return index * 2 + 1;
        }

        // 返回右子节点的索引
        private int RightChild(int index)
        {
            return index * 2 + 2;
        }

        // 添加元素
        public void Push(T e)
        {
            _data.AddLast(e);
            SiftUp(Count - 1);
        }

        // 弹出堆顶元素
        public T Pop()
        {
            T res = Peek();

            _data.Swap(0, Count - 1);
            _data.RemoveLast();
            SiftDown(0);

            return res;
        }

        // 查看堆顶元素
        public T Peek()
        {
            if (_data.IsEmpty)
                throw new InvalidOperationException();
            return _data[0];
        }

        // 弹出堆顶元素，然后替换
        public T Replace(T e)
        {
            T res = Peek();
            
            _data[0] = e;
            SiftDown(0);
            
            return res;
        }

        // 元素上浮
        private void SiftUp(int index)
        {
            while(index > 0 && _comparer.Compare(_data[index], _data[Parent(index)]) > 0)
            {
                _data.Swap(index, Parent(index));
                index = Parent(index);
            }
        }

        // 元素下沉
        private void SiftDown(int index)
        {
            while (LeftChild(index) < Count)
            {
                int j = LeftChild(index);
                if (j + 1 < Count && _comparer.Compare(_data[j + 1], _data[j]) > 0)
                    j = RightChild(index);

                if (_comparer.Compare(_data[j], _data[index]) < 0)
                    break;
                _data.Swap(index, j);
                index = j;
            }
        }

        public override string ToString()
        {
            return _data.ToString();
        }
    }
}
