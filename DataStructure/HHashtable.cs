using System;
using System.Collections.Generic;

namespace HDA.DataStructure
{
    internal class HHashtable<T>
    {
        private static readonly int[] _capacitys
            = {53, 97, 193, 389, 769, 1543, 3079, 6151, 12289, 24593,
            49157, 98317, 196613, 393241, 786433, 1572869, 3145739, 6291469,
            12582917, 25165843, 50331653, 100663319, 201326611, 402653189, 805306457, 1610612741};
        private static readonly int UpperTol = 10;
        private static readonly int LowerTol = 2;
        private int _capacityIndex = 0;

        private HSortedSet<T>[] _hashtable;
        private int _m;
        private int _count;

        public int Count => _count;
        public bool IsEmpty => _count == 0;

        private Comparer<T> _comparer = Comparer<T>.Default;
        public Comparer<T> Comparer => _comparer;

        public HHashtable()
        {
            _m = _capacitys[0];
            _count = 0;
            _hashtable = new HSortedSet<T>[_m];
            for (int i = 0; i < _m; i++)
                _hashtable[i] = new HSortedSet<T>(_comparer);
        }

        public HHashtable(Comparer<T> comparer)
        {
            _m = _capacitys[0];
            _count = 0;
            _hashtable = new HSortedSet<T>[_m];
            for (int i = 0; i < _m; i++)
                _hashtable[i] = new HSortedSet<T>(comparer);
        }

        private int Hash(T e)
        {
            return (e.GetHashCode() & 0x7fffffff) % _m;
        }

        public void Add(T e)
        {
            HSortedSet<T> map = _hashtable[Hash(e)];
            if (!map.Contains(e))
            {
                map.Add(e);
                _count++;

                if (_count >= UpperTol * _m && _capacityIndex + 1 < _capacitys.Length)
                {
                    _capacityIndex++;
                    Resize(_capacitys[_capacityIndex]);
                }
            }
        }

        public void Remove(T e)
        {
            HSortedSet<T> map = _hashtable[Hash(e)];
            if (map.Contains(e))
            {
                map.Remove(e);
                _count--;

                if (_count < LowerTol * _m && _capacityIndex - 1 >= 0)
                {
                    _capacityIndex--;
                    Resize(_capacitys[_capacityIndex]);
                }
            }
        }

        public bool Contains(T e)
        {
            return _hashtable[Hash(e)].Contains(e);
        }

        private void Resize(int newM)
        {
            HSortedSet<T>[] newHashtable = new HSortedSet<T>[newM];
            for (int i = 0; i < newM; i++)
                newHashtable[i] = new HSortedSet<T>(_comparer);
            int oldM = _m;
            _m = newM;
            for (int i = 0; i < oldM; i++)
            {
                HSortedSet<T> set = _hashtable[i];
                HList<T> hList = set.Keys;
                for (int j = 0; j < hList.Count; j++)
                    newHashtable[Hash(hList[i])].Add(hList[i]);
            }
            _hashtable = newHashtable;
        }
    }

    internal class HHashtable<K, V>
    {
        private static readonly int[] _capacitys
            = {53, 97, 193, 389, 769, 1543, 3079, 6151, 12289, 24593,
            49157, 98317, 196613, 393241, 786433, 1572869, 3145739, 6291469,
            12582917, 25165843, 50331653, 100663319, 201326611, 402653189, 805306457, 1610612741};
        private static readonly int UpperTol = 10;
        private static readonly int LowerTol = 2;
        private int _capacityIndex = 0;

        private HSortedDictionary<K, V>[] _hashtable;
        private int _m;
        private int _count;

        public int Count => _count;
        public bool IsEmpty => _count == 0;

        private Comparer<K> _comparer = Comparer<K>.Default;
        public Comparer<K> Comparer => _comparer;

        public V this[K key]
        {
            get
            {
                return _hashtable[Hash(key)][key];
            }
            set
            {
                HSortedDictionary<K, V> map = _hashtable[Hash(key)];
                if (!map.Contains(key))
                    throw new ArgumentException(key + "doesn't exist!");
                map[key] = value;
            }
        }

        public HHashtable()
        {
            _m = _capacitys[0];
            _count = 0;
            _hashtable = new HSortedDictionary<K, V>[_m];
            for (int i = 0; i < _m; i++)
                _hashtable[i] = new HSortedDictionary<K, V>(_comparer);
        }

        public HHashtable(Comparer<K> comparer)
        {
            _m = _capacitys[0];
            _count = 0;
            _hashtable = new HSortedDictionary<K, V>[_m];
            for (int i = 0; i < _m; i++)
                _hashtable[i] = new HSortedDictionary<K, V>(comparer);
        }

        private int Hash(K key)
        {
            return (key.GetHashCode() & 0x7fffffff) % _m;
        }

        public void Add(K key, V value)
        {
            HSortedDictionary<K, V> map = _hashtable[Hash(key)];
            if (map.Contains(key))
                map[key] = value;
            else
            {
                map.Add(key, value);
                _count++;

                if (_count >= UpperTol * _m && _capacityIndex + 1 < _capacitys.Length)
                {
                    _capacityIndex++;
                    Resize(_capacitys[_capacityIndex]);
                }
            }
        }

        public V Remove(K key)
        {
            HSortedDictionary<K, V> map = _hashtable[Hash(key)];
            V res = default;
            if (map.Contains(key))
            {
                res = map[key];
                map.Remove(key);
                _count--;

                if (_count < LowerTol * _m && _capacityIndex - 1 >= 0)
                {
                    _capacityIndex--;
                    Resize(_capacitys[_capacityIndex]);
                }
            }
            return res;
        }

        public bool Contains(K key)
        {
            return _hashtable[Hash(key)].Contains(key);
        }

        private void Resize(int newM)
        {
            HSortedDictionary<K, V>[] newHashtable = new HSortedDictionary<K, V>[newM];
            for (int i = 0; i < newM; i++)
                newHashtable[i] = new HSortedDictionary<K, V>(_comparer);
            int oldM = _m;
            _m = newM;
            for (int i = 0; i < oldM; i++)
            {
                HSortedDictionary<K, V> map = _hashtable[i];
                HList<K> hList = map.Keys;
                for (int j = 0; j < hList.Count; j++)
                    newHashtable[Hash(hList[i])].Add(hList[i], map[hList[i]]);
            }
            _hashtable = newHashtable;
        }
    }
}
