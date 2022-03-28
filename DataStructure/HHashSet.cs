using System;
using System.Collections.Generic;

namespace HDA.DataStructure
{
    internal class HHashSet<T>
    {
        private HHashtable<T> _hHashtable;

        public int Count { get => _hHashtable.Count; }
        public bool IsEmpty { get => _hHashtable.IsEmpty; }

        private Comparer<T> _comparer = Comparer<T>.Default;
        public Comparer<T> Compare => _comparer;

        public HHashSet()
        {
            _hHashtable = new HHashtable<T>(_comparer);
        }

        public HHashSet(Comparer<T> comparer)
        {
            _hHashtable = new HHashtable<T>(comparer);
        }

        public void Add(T e)
        {
            _hHashtable.Add(e);
        }

        public bool Contains(T e)
        {
            return _hHashtable.Contains(e);
        }

        public void Remove(T e)
        {
            _hHashtable.Remove(e);
        }
    }
}
