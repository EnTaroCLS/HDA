using System;

namespace HDA.DataStructure
{
    internal class HUnionFind
    {
        private int[] _rank;
        private int[] _parent; // parent[i]表示第i个元素所指向的父节点

        public int Count => _rank.Length;

        public HUnionFind(int count)
        {
            _rank = new int[count];
            _parent = new int[count];

            // 初始化, 每一个parent[i]指向自己, 表示每一个元素自己自成一个集合
            for (int i = 0; i < count; i++)
            {
                _parent[i] = i;
                _rank[i] = 1;
            }
        }

        // 查找元素p所对应的集合编号
        private int Find(int p)
        {
            if (p < 0 || p >= _parent.Length)
                throw new ArgumentOutOfRangeException("index");

            while (p != _parent[p])
            {
                _parent[p] = _parent[_parent[p]];
                p = _parent[p];
            }
            return p;
        }

        // 查看元素p和元素q是否所属一个集合
        public bool IsConnected(int p, int q)
        {
            return Find(p) == Find(q);
        }

        // 合并元素p和元素q所属的集合
        public void Union(int p, int q)
        {
            int pRoot = Find(p);
            int qRoot = Find(q);

            if (pRoot == qRoot)
                return;

            // 根据两个元素所在树的rank不同判断合并方向
            // 将rank低的集合合并到rank高的集合上
            if (_rank[pRoot] < _rank[qRoot])
                _parent[pRoot] = qRoot;
            else if (_rank[qRoot] < _rank[pRoot])
                _parent[qRoot] = pRoot;
            else
            {
                _parent[pRoot] = qRoot;
                _rank[qRoot] += 1;
            }
        }
    }
}
