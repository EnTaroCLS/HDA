using System;
using System.Text;

namespace HDA.DataStructure
{
    internal class HList<T>
    {
        private T[] _data;
        private int _count;

        public int Count { get => _count; private set => _count = value; }
        public int Capacity { get => _data.Length; }
        public bool IsEmpty { get => _count == 0; }
        
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("index");
                return _data[index];
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("index");
                _data[index] = value;
            }
        }

        public HList(int capacity)
        {
            _data = new T[capacity];
            Count = 0;
        }
        public HList() : this(10) { }

        public HList(T[] array)
        {
            _data = new T[array.Length];
            Array.Copy(array, 0, _data, 0, array.Length);
            _count = array.Length;
        }

        // 在index索引的位置插入一个新元素e
        public void Add(int index, T e)
        {
            if (index < 0 || index > _count)
                throw new ArgumentOutOfRangeException("index");

            if (_count == _data.Length)
                Resize(2 * _data.Length);

            for (int i = _count - 1; i >= index; i--)
                _data[i + 1] = _data[i];

            _data[index] = e;
            _count++;
        }

        // 向所有元素后添加一个新元素
        public void AddLast(T e)
        {
            Add(_count, e);
        }

        // 在所有元素前添加一个新元素
        public void AddFirst(T e)
        {
            Add(0, e);
        }

        // 查找数组中是否有元素e
        public bool Contains(T e)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_data[i].Equals(e))
                    return true;
            }
            return false;
        }

        // 查找数组中元素e所在的索引，如果不存在元素e，则返回-1
        public int Find(T e)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_data[i].Equals(e))
                    return i;
            }
            return -1;
        }

        // 从数组中删除index位置的元素, 返回删除的元素
        public T Remove(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException("index");

            T ret = _data[index];
            for (int i = index + 1; i < _count; i++)
                _data[i - 1] = _data[i];
            _count--;
            _data[_count] = default;

            if (_count == _data.Length / 4 && _data.Length / 2 != 0)
                Resize(_data.Length / 2);
            return ret;
        }

        // 从数组中删除第一个元素, 返回删除的元素
        public T RemoveFirst()
        {
            return Remove(0);
        }

        // 从数组中删除最后一个元素, 返回删除的元素
        public T RemoveLast()
        {
            return Remove(_count - 1);
        }

        // 从数组中删除元素e
        public void RemoveElement(T e)
        {
            int index = Find(e);
            if (index != -1)
                Remove(index);
        }

        public void Swap(int i, int j)
        {
            if (i < 0 || i >= _count || j < 0 || j >= _count)
                throw new ArgumentOutOfRangeException("i, j");
            T tmp = _data[i];
            _data[i] = _data[j];
            _data[j] = tmp;
        }

        public void Clear()
        {
            Array.Clear(_data, 0, _count);
            _count = 0;
        }

        // 将数组空间的容量变成newCapacity大小
        private void Resize(int newCapacity)
        {
            T[] newData = new T[newCapacity];
            for (int i = 0; i < _count; i++)
                newData[i] = _data[i];
            _data = newData;
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append(String.Format("HList: Count = {0} , Capacity = {1}\n", _count, _data.Length));
            res.Append('[');
            for (int i = 0; i < _count; i++)
            {
                res.Append(_data[i]);
                if (i != _count - 1)
                    res.Append(", ");
            }
            res.Append(']');
            return res.ToString();
        }
    }
}
