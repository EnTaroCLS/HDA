using System;
using System.Text;

namespace HDA.DataStructure
{
    internal class HQueue<T>
    {
        private T[] _data;
        private int _front, _tail;
        private int _count;

        public int Capacity { get => _data.Length; }
        public bool IsEmpty { get => _count == 0;}
        public int Count { get => _count; private set => _count = value; }

        public HQueue(int capacity)
        {
            _data = new T[capacity];
            _front = 0;
            _tail = 0;
            _count = 0;
        }

        public HQueue() : this(10) { }

        public void Enqueue(T e)
        {
            if (_count == Capacity)
                Resize(Capacity * 2);

            _data[_tail] = e;
            _tail = (_tail + 1) % _data.Length;
            _count++;
        }

        public T Dequeue()
        {
            if (IsEmpty)
                throw new Exception("Cannot dequeue from an empty queue.");

            T ret = _data[_front];
            _data[_front] = default;
            _front = (_front + 1) % _data.Length;
            _count--;
            if (_count == Capacity / 4 && Capacity / 2 != 0)
                Resize(Capacity / 2);
            return ret;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new Exception("Queue is empty.");
            return _data[_front];
        }

        private void Resize(int newCapacity)
        {

            T[] newData = new T[newCapacity];
            for (int i = 0; i < _count; i++)
                newData[i] = _data[(i + _front) % _data.Length];

            _data = newData;
            _front = 0;
            _tail = _count;
        }

        public void Clear()
        {
            Array.Clear(_data, 0, _count);
            _front = 0;
            _tail = 0;
            _count = 0;
        }

        public override String ToString()
        {

            StringBuilder res = new StringBuilder();
            res.Append(String.Format("Queue: size = {0} , capacity = {1}\n", _count, Capacity));
            res.Append("front [");

            for (int i = 0; i < _count; i++)
            {
                res.Append(_data[(_front + i) % _data.Length]);
                if ((i + _front + 1) % _data.Length != _tail)
                    res.Append(", ");
            }
            res.Append("] tail");
            return res.ToString();
        }
    }
}
