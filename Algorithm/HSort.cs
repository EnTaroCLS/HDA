using System;
using System.Collections.Generic;
using HDA.DataStructure;

namespace HDA.Algorithm
{
    internal static class HSort
    {
        #region 选择排序
        public static void SelectSort<T>(this HList<T> arr)
        {
            Comparer<T> comparer = Comparer<T>.Default;
            arr.SelectSort(comparer);
        }

        public static void SelectSort<T>(this HList<T> arr, Comparer<T> comparer)
        {
            for (int i = 0; i < arr.Count - 1; i++)
            {
                int index = i;
                for (int j = i + 1; j < arr.Count; j++)
                {
                    if (comparer.Compare(arr[j], arr[index]) < 0)
                        index = j;
                }
                arr.Swap(index, i);
            }
        }
        #endregion

        #region 插入排序
        public static void InsertSort<T>(this HList<T> arr)
        {
            Comparer<T> comparer = Comparer<T>.Default;
            arr.InsertSort(comparer);
        }

        public static void InsertSort<T>(this HList<T> arr, Comparer<T> comparer)
        {
            for (int i = 1; i < arr.Count; i++)
            {
                T tmp = arr[i];
                int j;
                for (j = i - 1; j >= 0; j--)
                {
                    if (comparer.Compare(arr[j], tmp) > 0)
                        arr[j + 1] = arr[j];
                    else
                        break;
                }
                arr[j + 1] = tmp;
            }
        }
        #endregion

        #region 冒泡排序
        public static void BubbleSort<T>(this HList<T> arr)
        {
            Comparer<T> comparer = Comparer<T>.Default;
            arr.BubbleSort(comparer);
            
        }

        public static void BubbleSort<T>(this HList<T> arr, Comparer<T> comparer)
        {
            for (int i = 0; i < arr.Count - 1;)
            {
                int swapIndex = 0;
                for (int j = 1; j < arr.Count - i; j++)
                {
                    if (comparer.Compare(arr[j], arr[j - 1]) < 0)
                    {
                        arr.Swap(j, j - 1);
                        swapIndex = j;
                    }
                }
                i = arr.Count - swapIndex;
            }
        }
        #endregion

        #region 归并排序
        public static void MergeSort<T>(this HList<T> arr)
        {
            Comparer<T> comparer = Comparer<T>.Default;
            T[] mergeArr = new T[arr.Count];
            arr.MergeSort(0, arr.Count - 1, mergeArr, comparer);
        }

        public static void MergeSort<T>(this HList<T> arr, Comparer<T> comparer)
        {
            T[] mergeArr = new T[arr.Count];
            arr.MergeSort(0, arr.Count - 1, mergeArr, comparer);
        }

        private static void MergeSort<T>(this HList<T> arr, int l, int r, T[] mergeArr, Comparer<T> comparer)
        {
            if (l >= r)
                return;

            // 用插入排序优化小规模数据
            if (r - l < 16)
            {
                for (int i = l + 1; i <= r; i++)
                {
                    T t = arr[i];
                    int j;
                    for (j = i - 1; j >= l; j--)
                    {
                        if (comparer.Compare(arr[j], t) > 0)
                            arr[j + 1] = arr[j];
                        else
                            break;
                    }
                    arr[j + 1] = t;
                }
                return;
            }

            int mid = l + (r - l) / 2;
            arr.MergeSort(l, mid, mergeArr, comparer);
            arr.MergeSort(mid + 1, r, mergeArr, comparer);

            if (comparer.Compare(arr[mid], arr[mid + 1]) <= 0)
                return;

            int leftIndex = l;
            int rightIndex = mid + 1;
            for (int i = 0; i < r - l + 1; i++)
            {
                if (leftIndex > mid)
                    mergeArr[i] = arr[rightIndex++];
                else if(rightIndex > r)
                    mergeArr[i] = arr[leftIndex++];
                else
                {
                    if (comparer.Compare(arr[leftIndex], arr[rightIndex]) < 0)
                        mergeArr[i] = arr[leftIndex++];
                    else
                        mergeArr[i] = arr[rightIndex++];
                }
            }
            for (int i = l; i <= r; i++)
                arr[i] = mergeArr[i - l];
        }
        #endregion

        #region 快速排序
        public static void QuickSort<T>(this HList<T> arr)
        {
            Comparer<T> comparer = Comparer<T>.Default;
            Random random = new Random();
            arr.QuickSort(0, arr.Count - 1, comparer, random);
        }

        public static void QuickSort<T>(this HList<T> arr, Comparer<T> comparer)
        {
            Random random = new Random();
            arr.QuickSort(0, arr.Count - 1, comparer, random);
        }

        private static void QuickSort<T>(this HList<T> arr, int l, int r, Comparer<T> comparer, Random random)
        {
            if (l >= r)
                return;

            random = new Random();
            arr.Swap(l, random.Next(l, r + 1));
            int lt = l + 1, gt = r + 1, i = l + 1;
            while (i < gt)
            {
                if (comparer.Compare(arr[i], arr[l]) > 0)
                    arr.Swap(i, --gt);
                else if (comparer.Compare(arr[i], arr[l]) < 0)
                    arr.Swap(i++, lt++);
                else
                    i++;
            }
            arr.Swap(lt - 1, l);

            arr.QuickSort(l, lt - 1, comparer, random);
            arr.QuickSort(gt, r, comparer, random);
        }
        #endregion

        #region 希尔排序
        public static void ShellSort<T>(this HList<T> arr)
        {
            Comparer<T> comparer = Comparer<T>.Default;
            arr.ShellSort(comparer);
        }

        public static void ShellSort<T>(this HList<T> arr, Comparer<T> comparer)
        {
            // 步长
            int h = 1;
            while (h < arr.Count / 3)
                h = h * 3 + 1;

            while (h > 0)
            {
                for (int i = h; i < arr.Count; i++)
                {
                    T tmp = arr[i];
                    int j;
                    for (j = i - h; j >=0 && comparer.Compare(arr[j], tmp) > 0; j -= h)
                        arr[j + h] = arr[j];
                    arr[j + h] = tmp;
                }
                h /= 3;
            }
        }
        #endregion

        #region 堆排序
        public static void HeapSort<T>(this HList<T> arr)
        {
            Comparer<T> comparer = Comparer<T>.Default;
            arr.HeapSort(comparer);
        }

        public static void HeapSort<T>(this HList<T> arr, Comparer<T> comparer)
        {
            arr.Heapify(comparer);
            for (int i = 1; i < arr.Count; i++)
            {
                arr.Swap(0, arr.Count - i);
                arr.SiftDown(arr.Count - i, 0, comparer);
            }
        }

        private static void Heapify<T>(this HList<T> arr, Comparer<T> comparer)
        {
            if (arr.Count < 1)
                return;

            for (int i = Parent(arr.Count - 1); i >= 0; i--)
                arr.SiftDown(arr.Count, i, comparer);
        }

        private static void SiftDown<T>(this HList<T> arr, int r, int index, Comparer<T> comparer)
        {
            while (LeftChild(index) < r)
            {
                int j = LeftChild(index);
                if (j + 1 < r && comparer.Compare(arr[j + 1], arr[j]) > 0)
                    j++;
                if (comparer.Compare(arr[index], arr[j]) < 0)
                    arr.Swap(index, j);
                index = j;
            }
        }

        private static int Parent(int index)
        {
            return (index - 1) / 2;
        }

        private static int LeftChild(int index)
        {
            return 2 * index + 1;
        }
        #endregion

        #region 桶排序
        public static void BucketSort(this int[] arr)
        {
            arr.BucketSort(200);
        }

        public static void BucketSort(this int[] arr, int c)
        {
            if (c < 1)
                throw new ArgumentOutOfRangeException("c");

            int maxv = int.MinValue, minv = int.MaxValue;
            foreach (int e in arr)
            {
                maxv = Math.Max(maxv, e);
                minv = Math.Min(minv, e);
            }

            int range = maxv - minv + 1; // arr 中的数据范围
            int B = range / c + (range % c > 0 ? 1 : 0); // 根据数据范围决定桶的个数

            HList<int>[] buckets = new HList<int>[B];
            for (int i = 0; i < B; i++)
                buckets[i] = new HList<int>();

            foreach (int e in arr)
                buckets[(e - minv) / c].AddLast(e);

            for (int i = 0; i < B; i++)
                buckets[i].InsertSort();

            for (int i = 0; i < B; i++)
                Console.WriteLine(buckets[i]);

            int index = 0;
            for (int i = 0; i < B; i++)
                for (int j = 0; j < buckets[i].Count; j++)
                    arr[index++] = buckets[i][j];
        }
        #endregion

        #region MSD基数排序
        public static void MSDSort(string[] arr)
        {
            int N = arr.Length;
            String[] temp = new String[N];
            MSDSort(arr, 0, N - 1, 0, temp);
        }

        private static void MSDSort(string[] arr, int left, int right, int r, String[] temp)
        {
            if (left >= right) return;

            int R = 256;
            int[] cnt = new int[R + 1];
            int[] index = new int[R + 2];

            for (int i = left; i <= right; i++)
                cnt[r >= arr[i].Length ? 0 : (arr[i][r] + 1)]++;

            for (int i = 0; i < R + 1; i++)
                index[i + 1] = index[i] + cnt[i];

            for (int i = left; i <= right; i++)
            {
                temp[index[r >= arr[i].Length ? 0 : (arr[i][r] + 1)] + left] = arr[i];
                index[r >= arr[i].Length ? 0 : (arr[i][r] + 1)]++;
            }

            for (int i = left; i <= right; i++)
                arr[i] = temp[i];

            for (int i = 0; i < R; i++)
                MSDSort(arr, left + index[i], left + index[i + 1] - 1, r + 1, temp);
        }
        #endregion

    }
}
