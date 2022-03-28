using System;
using System.Collections.Generic;

namespace HDA.DataStructure
{
    internal class HDictionary<K, V>
    {
        private HHashtable<K, V> _hHashtable;

        public int Count { get => _hHashtable.Count; }
        public bool IsEmpty { get => _hHashtable.IsEmpty; }

        private Comparer<K> _comparer = Comparer<K>.Default;
        public Comparer<K> Compare => _comparer;

        public V this[K key]
        {
            get => _hHashtable[key];
            set => _hHashtable[key] = value;
        }

        public HDictionary()
        {
            _hHashtable = new HHashtable<K, V>(_comparer);
        }

        public HDictionary(Comparer<K> comparer)
        {
            _hHashtable = new HHashtable<K, V>(comparer);
        }

        public void Add(K key, V value)
        {
            _hHashtable.Add(key, value);
        }

        public bool Contains(K key)
        {
            return _hHashtable.Contains(key);
        }

        public void Remove(K key)
        {
            _hHashtable.Remove(key);
        }
    }
}
