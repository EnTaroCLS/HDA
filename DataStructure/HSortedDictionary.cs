using System;
using System.Collections.Generic;

namespace HDA.DataStructure
{
    internal class HSortedDictionary<K, V>
    {
        private HRBTree<K, V> hRBTree;

        public int Count { get => hRBTree.Count; }
        public bool IsEmpty { get => hRBTree.IsEmpty; }

        public HList<K> Keys
        {
            get
            {
                HList<K> hList = new HList<K>();
                hRBTree.InOrder(x => hList.AddLast(x));
                return hList;
            }
        }

        private Comparer<K> _comparer = Comparer<K>.Default;
        public Comparer<K> Compare => _comparer;

        public V this[K key]
        {
            get => hRBTree[key];
            set => hRBTree[key] = value;
        }

        public HSortedDictionary()
        {
            hRBTree = new HRBTree<K, V>(_comparer);
        }

        public HSortedDictionary(Comparer<K> comparer)
        {
            hRBTree = new HRBTree<K, V>(comparer);
        }

        public void Add(K key, V value)
        {
            hRBTree.Add(key, value);
        }

        public bool Contains(K key)
        {
            return hRBTree.Contains(key);
        }

        public void Remove(K key)
        {
            hRBTree.Remove(key);
        }
    }
}
