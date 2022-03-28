using System;

namespace HDA.DataStructure
{
    internal class HSegmentTree<T>
    {
        private T[] _data;
        private T[] _tree;
        private Func<T, T, T> _merger;

        public int Count => _data.Length;

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

                _data[index] = value;
                Update(0, 0, Count - 1, index, value);
            }
        }

        public HSegmentTree(T[] arr, Func<T, T, T> merger)
        {
            _data = new T[arr.Length];
            arr.CopyTo(_data, 0);
            _merger = merger;

            _tree = new T[arr.Length * 4];
            BulidSegmentTree(0, 0, Count - 1);
        }

        // 用完全二叉树数组建立线段树
        private void BulidSegmentTree(int treeIndex, int l, int r)
        {
            if (l == r)
            {
                _tree[treeIndex] = _data[l];
                return;
            }

            int leftTreeIndex = LeftChild(treeIndex);
            int rightTreeIndex = RightChild(treeIndex);
            
            int mid = l + (r - l) / 2;
            BulidSegmentTree(leftTreeIndex, l, mid);
            BulidSegmentTree(rightTreeIndex, mid + 1, r);

            _tree[treeIndex] = _merger(_tree[leftTreeIndex], _tree[rightTreeIndex]);
        }

        // 返回[queryL, queryR]的值
        public T QueryRange(int queryL, int queryR)
        {
            if (queryL > queryR || queryL < 0 || queryR >= Count)
                throw new ArgumentOutOfRangeException(nameof(queryL), nameof(queryR));

            return QueryRange(0, 0, Count - 1, queryL, queryR);
        }

        // 返回以treeIndex为根节点的线段树中，搜[queryL, queryR]的值
        private T QueryRange(int treeIndex, int l, int r, int queryL, int queryR)
        {
            if (l == queryL && r == queryR)
                return _tree[treeIndex];

            int mid = l + (r - l) / 2;
            int leftTreeIndex = LeftChild(treeIndex);
            int rightTreeIndex = RightChild(treeIndex);

            if (queryL > mid)
                return QueryRange(rightTreeIndex, mid + 1, r, queryL, queryR);
            else if (queryR <= mid)
                return QueryRange(leftTreeIndex, l, mid, queryL, queryR);

            T leftResult = QueryRange(leftTreeIndex, l, mid, queryL, mid);
            T rightResult = QueryRange(rightTreeIndex, mid + 1, r, mid + 1, queryR);

            return _merger(leftResult, rightResult);
        }

        private int LeftChild(int index)
        {
            return 2 * index + 1;
        }

        private int RightChild(int index)
        {
            return 2 * index + 2;
        }

        private void Update(int treeIndex, int l, int r, int index, T value)
        {
            if (l == r)
            {
                _tree[treeIndex] = value;
                return;
            }

            int mid = l + (r - l) / 2;
            int leftTreeIndex = LeftChild(treeIndex);
            int rightTreeIndex = RightChild(treeIndex);
            if (index > mid)
                Update(leftTreeIndex, mid + 1, r, index, value);
            else
                Update(rightTreeIndex, l, mid, index, value);

            _tree[treeIndex] = _merger(_tree[leftTreeIndex], _tree[rightTreeIndex]);
        }
    }
}
