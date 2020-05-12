using System;
using System.Collections.Generic;
using System.Linq;

namespace RecursionAndSorting
{
    class Program
    {
        //Merge sort
        static void Main()
        {
            // Task01();

            // Task02();

            // Task03();

            Task04();
        }

        private static void Task04()
        {
            var wm = new WordGenerator();
            var symbols = Console.ReadLine().ToCharArray();
            MergeSorter.MergeSort(symbols);
            var result = wm.GetWords(symbols);
            Console.WriteLine(result.Count());
        }

        public class WordGenerator
        {
            public HashSet<string> GetWords(char[] sortedArr)
            {
                var bank = new HashSet<string>();
                GetWordCombs(sortedArr, 0, sortedArr.Length - 1, bank);
                return bank;
            }
            private void GetWordCombs(char[] word, int start, int end, HashSet<string> bank)
            {
                if (IsValidWord(word)) bank.Add(string.Join("", word));
                for (int left = end - 1; left >= start; left--)
                {
                    for (int right = left + 1; right <= end; right++)
                        if (word[left] != word[right])
                        {
                            Swap(word, left, right);
                            GetWordCombs(word, left + 1, end, bank);
                        }
                    var firstElement = word[left];
                    for (int i = left; i <= end - 1; i++)
                    {
                        word[i] = word[i + 1];
                    }
                    word[end] = firstElement;
                }
            }

            private void Swap(char[] word, int left, int right)
            {
                var temp = word[left];
                word[left] = word[right];
                word[right] = temp;
            }

            private bool IsValidWord(char[] word)
            {
                for (int i = 1; i < word.Length; i++)
                {
                    if (word[i - 1] == word[i]) return false;
                }
                return true;
            }
        }

        private static void Task03()
        {
            var array = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            Console.WriteLine(MergeSorter.GetInversionsCount(array));
        }

        private static void Task02()
        {
            var array = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            int target = int.Parse(Console.ReadLine());
            Console.WriteLine(BinarySearcher.GetIndexOf(array, target));
        }
        public static void Task01()
        {
            var array = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            MergeSorter.MergeSort(array);
            Console.WriteLine(string.Join(" ", array));
        }

        public static class BinarySearcher
        {
            public static int GetIndexOf<T>(T[] arr, T elmnt) where T : IComparable
            {
                return GetIndex(arr, elmnt, 0, arr.Length - 1);
            }

            private static int GetIndex<T>(T[] arr, T element, int start, int end) where T : IComparable
            {
                if (end == start)
                {
                    if (arr[start].CompareTo(element) == 0) return start;
                    else return -1;
                }

                int middle = (start + end) / 2;
                if (arr[middle].CompareTo(element) == 0) return middle;
                if (arr[middle].CompareTo(element) == 1) return GetIndex(arr, element, start, middle - 1);
                else return GetIndex(arr, element, middle + 1, end);
            }
        }
        public static class MergeSorter
        {
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

            public static int GetInversionsCount<T>(T[] arr)
                where T : IComparable
            {
                int invs = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    for (int j = i + 1; j < arr.Length; j++)
                    {
                        if (arr[i].CompareTo(arr[j]) == 1) invs++;
                    }
                }
                return invs;
            }


            public static int GetInversionsCountRecursively<T>(T[] arr, int index = 0)
                   where T : IComparable
            {
                if (index == arr.Length - 1) return 0;
                var current = arr[index];
                int invs = 0;
                for (int i = index + 1; i < arr.Length; i++)
                {
                    if (current.CompareTo(arr[i]) == 1) invs++;
                }
                return invs + GetInversionsCountRecursively(arr, index + 1);
            }
        }
    }
}
