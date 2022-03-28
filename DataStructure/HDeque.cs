using System;
using System.Text;

namespace HDA.DataStructure
{
    internal class HDeque<T>
    {
        private T[] _data;
        private int _front, _tail;
        private int _count;

        public int Count => _count;
        public int Capacity => _data.Length;
        public bool IsEmpty => _data.Length == 0;

        public HDeque(int capacity)
        {
            _data = new T[capacity];
            _front = 0;
            _tail = 0;
            _count = 0;
        }
        public HDeque() : this(10) { }

        public void Enqueue(T e)
        {
            if (_count == Capacity)
                Resize(Capacity * 2);

            _data[_tail] = e;
            _tail = (_tail + 1) % _data.Length;
            _count++;
        }

        public void AddFront(T e)
        {
            if (_count == Capacity)
                Resize(Capacity * 2);

            _front = _front == 0 ? _data.Length - 1 : _front - 1;
            _data[_front] = e;
            _count++;
        }

        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            T res = _data[_front];
            _data[_front] = default;
            _front = (_front + 1) % _data.Length;
            _count--;
            if (Count == Capacity / 4 && Capacity / 2 != 0)
                Resize(Capacity / 2);
            return res;
        }

        public T RemoveLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            _tail = _tail == 0 ? _data.Length - 1 : _tail - 1;
            T res = _data[_tail];
            _data[_tail] = default;
            _count--;
            if (Count == Capacity / 4 && Capacity / 2 != 0)
                Resize(Capacity / 2);
            return res;
        }

        public T PeekFirst()
        {
            if (IsEmpty)
                throw new InvalidOperationException();
            return _data[_front];
        }

        public T PeekLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            int index = _tail == 0 ? _data.Length - 1 : _tail - 1;
            return _data[index];
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

        public override String ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append(String.Format("Queue: size = {0} , capacity = {1}\n", Count, Capacity));
            res.Append("front [");
            for (int i = 0; i < _count; i++)
            {
                res.Append(_data[(i + _front) % _data.Length]);
                if (i != _count - 1)
                    res.Append(", ");
            }
            res.Append("] tail");
            return res.ToString();
        }
    }
}
