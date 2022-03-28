using System;
using System.Collections.Generic;
using HDA.DataStructure;

namespace HDA.Algorithm
{
    internal static class HSearch
    {
        #region Rabin-Karp 字符串匹配算法
        public static long RKSearch(this string s, string t)
        {
            if (s == null || t == null)
                throw new ArgumentNullException(nameof(s), nameof(t));
            if (s.Length < t.Length)
                return -1;
            if (t.Length == 0)
                return 0;

            long tHash = 0;
            const long mod = (long)1e9+7;
            const int b = 256;

            for (int i = 0; i < t.Length; i++)
                tHash = (tHash * b + t[i]) % mod;

            long hash = 0, p = 1;
            for (int i = 0; i < t.Length - 1; i++)
                p = (p * b) % mod;

            for (int i = 0; i < t.Length - 1; i++)
                hash = (hash * b + s[i]) % mod;

            for (int i = t.Length - 1; i < s.Length; i++)
            {
                hash = (hash * b + s[i]) % mod;
                if (hash == tHash && s.Equal(i - t.Length + 1, t))
                    return i - t.Length + 1;

                hash = (hash - s[i - t.Length + 1] * p % mod + mod) % mod;
            }

            return -1;
        }

        private static bool Equal(this string s, int l, string t)
        {
            for (int i = 0; i < t.Length; i++)
                if (s[l + i] != t[i])
                    return false;
            return true;
        }
        #endregion
        
        #region KMP 字符串匹配算法
        public static int KMPSearch(this string s, string t)
        {
            int[] next = Next(t);
            int i = 0, j = -1;

            while (i < s.Length)
            {
                if (j == -1 || s[i] == t[j])
                {
                    i++;
                    j++;
                    if (j > t.Length - 1)
                        return i - j;
                }
                else
                    j = next[j];
            }

            return -1;
        }

        private static int[] Next(string t)
        {
            int[] next = new int[t.Length];
            int i = 0, j = -1;
            next[0] = -1;

            while (i < t.Length - 1)
            {
                if (j == -1 || t[i] == t[j])
                {
                    i++;
                    j++;
                    if (i > 0)
                        next[i] = j;
                }
                else
                    j = next[j];
            }
            for (int m = 0; m < next.Length; m++)
            {
                int k = next[m];
                if (next[m] == -1)
                    continue;
                if (t[m] == t[k])
                    next[m] = next[k];
            }
            return next;
        }

        #endregion

        public static T Max<T>(this HList<T> arr)
        {
            if (arr == null || arr.Count == 0)
                throw new ArgumentException("arr");

            Comparer<T> comparer = Comparer<T>.Default;
            T result = arr[0];
            for (int i = 1; i < arr.Count; i++)
                if (comparer.Compare(arr[i], result) > 0)
                    result = arr[i];
            return result;
        }

        public static T Min<T>(this HList<T> arr)
        {
            if (arr == null || arr.Count == 0)
                throw new ArgumentException("arr");

            Comparer<T> comparer = Comparer<T>.Default;
            T result = arr[0];
            for (int i = 1; i < arr.Count; i++)
                if (comparer.Compare(arr[i], result) < 0)
                    result = arr[i];
            return result;
        }
    }
}
