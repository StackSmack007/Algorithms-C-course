using System;
using System.Linq;
using System.Collections.Generic;
namespace NeedlesTask
{
    public class Program
    {
        static void Main()
        {
            Console.ReadLine();
            int[] numbers = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var needles = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var resultIndexes = new Stack<int>(needles.Count());



            GetInserionIndexesBinary(resultIndexes, numbers, needles);
            Console.WriteLine(string.Join(" ", resultIndexes.Reverse()));
        }

        private static void GetInserionIndexes(Stack<int> result, int[] numbers, int[] needles)
        {
            for (int i = 0; i < needles.Length; i++)
            {
                int curNeedle = needles[i];
                int index;

                for (index = 0; index < numbers.Count(); index++)
                {
                    if (numbers[index] == 0) continue;
                    if (numbers[index] >= curNeedle) break;
                }

                while (index > 0 && numbers[index - 1] == 0) index--;
                result.Push(index);
            }
        }


        private static void GetInserionIndexesBinary(Stack<int> result, int[] numbers, int[] needles)
        {
            var meaningFullIndexes = GetMeaningFullIndexes(numbers).ToArray();
            for (int i = 0; i < needles.Length; i++)
            {
                int curNeedle = needles[i];
                int index = GetFirstGreater(meaningFullIndexes, numbers, curNeedle);

                while (index > 0 && (numbers[index - 1] == 0 || numbers[index - 1] == curNeedle)) index--;
                result.Push(index);
            }
        }


        private static IEnumerable<int> GetMeaningFullIndexes(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != 0) yield return i;
            }
        }

        public static int GetFirstGreater<T>(int[] indexes, T[] values, T elmnt) where T : IComparable
        {
            return GetFirstGreater(indexes, values, elmnt, 0, indexes.Length - 1);
        }

        private static int GetFirstGreater<T>(int[] indexes, T[] values, T element, int start, int end) where T : IComparable
        {
            if (end == start)
            {
                if (values[indexes[start]].CompareTo(element) == 1) return indexes[start];
                else return indexes.Last()+1;
            }

            int middle = (start + end) / 2;
            if (values[indexes[middle]].CompareTo(element) == 1) return GetFirstGreater(indexes, values, element, start, middle);
            else return GetFirstGreater(indexes, values, element, middle + 1, end);
        }
    }
}