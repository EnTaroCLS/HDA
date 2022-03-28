using System;

namespace HDA.DataStructure
{
    internal class HSqrtDecomposition<T>
    {
        private T[] _data, _blocks;
        private int _count;  // 元素总数
        private int _b;  // 每组元素个数
        private int _blockCount; // 组数
        private Func<T, T, T> _merger;

        public int Count => _count;
        public bool IsEmpty => _count == 0;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index");
                return _data[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index");
                if (value == null)
                    throw new ArgumentNullException("value");

                int b = index / _b;
                _data[index] = value;

                _blocks[b] = _data[b * _b];
                for (int i = b * _b + 1; i < Math.Min((b + 1) * _b, _count); i++)
                    _blocks[b] = _merger(_blocks[b], _data[i]);
            }
        }

        public HSqrtDecomposition(T[] arr, Func<T, T, T> merger)
        {
            _merger = merger;

            _count = arr.Length;
            if (_count == 0) 
                return;

            _b = (int)Math.Sqrt(_count);
            _blockCount = _count / _b + (_count % _b > 0 ? 1 : 0);

            _data = new T[_count];
            Array.Copy(arr, 0, _data, 0, _count);

            _blocks = new T[_blockCount];
            for (int i = 0; i < _count; i++)
                if (i % _b == 0)
                    _blocks[i / _b] = _data[i];
                else
                    _blocks[i / _b] = _merger(_blocks[i / _b], _data[i]);
        }

        /// 区间查询
        public T QueryRange(int x, int y)
        {
            if (x < 0 || x >= _count || y < 0 || y >= _count || x > y) 
                return default;

            int bstart = x / _b, bend = y / _b;

            T res = _data[x];
            if (bstart == bend)
            {
                for (int i = x + 1; i <= y; i++)
                    res = _merger(res, _data[i]);
                return res;
            }

            for (int i = x + 1; i < (bstart + 1) * _b; i++)
                res = _merger(res, _data[i]);
            for (int b = bstart + 1; b < bend; b++)
                res = _merger(res, _blocks[b]);
            for (int i = bend * _b; i <= y; i++)
                res = _merger(res, _data[i]);
            return res;
        }
    }
}
