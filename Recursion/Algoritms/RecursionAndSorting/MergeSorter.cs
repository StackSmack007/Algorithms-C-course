namespace RecursionAndSorting
{
    using System;
    using System.Collections.Generic;

    public static class MergeSorter
    {
        public static int GetInversionsCount<T>(T[] arr, int index = 0)
                       where T : IComparable
        {
            if (index == arr.Length - 1) return 0;
            var current = arr[index];
            var invs = 0;
            for (int i = index + 1; i < arr.Length; i++)
            {
                if (current.CompareTo(arr[i]) == 1) invs++;
            }
            return invs + GetInversionsCount(arr, index + 1);
        }
        public static void MergeSort<T>(T[] arr) where T : IComparable
                           => MergeSort(arr, 0, arr.Length - 1);
        private static void MergeSort<T>(T[] arr, int start, int end)
                       where T : IComparable
        {
            if (end == start
                || (end - start == 1 && arr[end].CompareTo(arr[start]) >= 0))
            {
                return;
            }

            if (end - start == 1)
            {
                var temp = arr[end];
                arr[end] = arr[start];
                arr[start] = temp;
            }
            else
            {
                int middle = (start + end) / 2;    //    5/2=>2 
                MergeSort(arr, start, middle);     //   0 1 2 indexes
                MergeSort(arr, middle + 1, end);   //   3 4
                Merge(arr, start, middle, end);
            }
        }
        private static void Merge<T>(T[] arr, int start, int middle, int end) where T : IComparable
        {
            int fIndex = start;
            int sIndex = middle + 1;
            var tempArr = new T[1 + end - start];

            for (int i = 0; i < tempArr.Length; i++)
            {
                //One of the arrays is over and elements are taken from other one!
                if (fIndex > middle) tempArr[i] = arr[sIndex++];
                else if (sIndex > end) tempArr[i] = arr[fIndex++];
                //asign the smaler and move it's index forward!
                else if (arr[fIndex].CompareTo(arr[sIndex]) <= 0) tempArr[i] = arr[fIndex++];
                else tempArr[i] = arr[sIndex++];
            }

            for (int i = 0; i < tempArr.Length; i++)
            {
                arr[i + start] = tempArr[i];
            }
        }
    }
}
