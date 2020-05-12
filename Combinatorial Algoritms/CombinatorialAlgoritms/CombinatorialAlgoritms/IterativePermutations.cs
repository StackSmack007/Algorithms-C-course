namespace CombinatorialAlgoritms
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    class IterativePermutations
    {
        public static void Run()
        {
            var input = Console.ReadLine().Split(' ');
            var result = Permute(input).ToArray();
            Console.WriteLine(string.Join(Environment.NewLine, result.Select(x => string.Join(" ", x))));
        }
        private static IList<T[]> Permute<T>(T[] input) where T : IComparable
        {
            var result = new List<T[]>();
            PermuteEngine(input.ToArray(), result);
            return result;
        }

        private static void PermuteEngine<T>(T[] arr, IList<T[]> bank, int start = 0) where T : IComparable
        {
            if (start >= arr.Length)
            {
                bank.Add(arr.ToArray());
                return;
            }

            PermuteEngine(arr, bank, start + 1);
            var used = new HashSet<T> { arr[start] };

            for (var i = start + 1; i < arr.Length; i++)
            {
                if (!used.Contains(arr[i]))
                {
                    used.Add(arr[i]);
                    SwapElements(arr, start, i);
                    PermuteEngine(arr, bank, start + 1);

                    SwapElements(arr, start, i);
                }
            }
        }
        private static void SwapElements<T>(T[] input, int ind1, int ind2 = -1)
        {
            ind2 = ind2 == -1 ? ind1 + 1 : ind2;
            var temp = input[ind1];
            input[ind1] = input[ind2];
            input[ind2] = temp;
        }

    }
    public class PermuteTemp
    {

        public static void Log()
        {
            var input = Console.ReadLine().Split(' ');
            var result = Permute(input).ToArray();
            Console.WriteLine(new string('-', 10));
            Console.WriteLine(string.Join(Environment.NewLine, result.Select(x => string.Join(" ", x))));
            Console.WriteLine(new string('-', 10));
            Console.WriteLine(result.Count());
            var set = new HashSet<string>(result.Select(x => string.Join(" ", x)));
            Console.WriteLine("Unique combos:" + set.Count());
        }

        private static IList<T[]> Permute<T>(T[] input) where T : IComparable
        {
            var swapCount = GetPermutationCount(input.Length);
            var result = new List<T[]>(swapCount);
            result.Add(input.ToArray());

            int swapIndex = 0;
            for (int i = 1; i < swapCount; i++)
            {
                swapIndex = (swapIndex + 1) % (input.Length - 1);
                SwapElements(input, swapIndex);
                result.Add(input.ToArray());
            }

            return result;
        }

        private static void SwapElements<T>(T[] input, int ind1, int ind2 = -1)
        {
            ind2 = ind2 == -1 ? ind1 + 1 : ind2;
            var temp = input[ind1];
            input[ind1] = input[ind2];
            input[ind2] = temp;
        }

        private static int GetPermutationCount(int elmCnt)
        {
            var swapCount = 1;
            for (int i = 2; i <= elmCnt; i++) swapCount *= i;
            return swapCount;
        }

        private static void PermuteRep<T>(T[] arr, IList<T[]> bank, int start, int end) where T : IComparable
        {
            bank.Add(arr.ToArray());
            for (int left = end - 1; left >= start; left--)
            {
                for (int right = left + 1; right < end; right++)
                {
                    if (arr[left].CompareTo(arr[right]) == 0) continue;

                    SwapElements(arr, left, right);
                    PermuteRep(arr, bank, left + 1, end);
                    //SwapElements(arr, left, right);
                }
                var firstElement = arr[left];
                for (int i = left; i <= end - 1; i++)
                    arr[i] = arr[i + 1];
                arr[end] = firstElement;
            }

        }


    }
}