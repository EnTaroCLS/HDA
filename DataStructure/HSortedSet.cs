using System;
using System.Collections.Generic;

namespace HDA.DataStructure
{
    internal class HSortedSet<T>
    {
        private HRBTree<T> hRBTree;

        public int Count { get => hRBTree.Count; }
        public bool IsEmpty { get => hRBTree.IsEmpty; }

        public HList<T> Keys
        {
            get
            {
                HList<T> hList = new HList<T>();
                hRBTree.InOrder(x => hList.AddLast(x));
                return hList;
            }
        }

        private Comparer<T> _comparer = Comparer<T>.Default;
        public Comparer<T> Compare => _comparer;

        public HSortedSet()
        {
            hRBTree = new HRBTree<T>(_comparer);
        }

        public HSortedSet(Comparer<T> comparer)
        {
            hRBTree = new HRBTree<T>(comparer);
        }

        public void Add(T e)
        {
            hRBTree.Add(e);
        }

        public bool Contains(T e)
        {
            return hRBTree.Contains(e);
        }

        public void Remove(T e)
        {
            hRBTree.Remove(e);
        }
    }
}
