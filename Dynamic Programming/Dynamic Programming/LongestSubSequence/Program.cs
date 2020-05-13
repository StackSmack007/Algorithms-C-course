using System;

namespace LongestSubSequence
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] first = Console.ReadLine().ToCharArray();
            char[] second = Console.ReadLine().ToCharArray();

            int[,] result = new int[first.Length + 1, second.Length + 1];

            for (int f = 1; f < result.GetLength(0); f++)
            {
                var currentFirst = first[f - 1];
                for (int s = 1; s < result.GetLength(1); s++)
                {
                    var currentSecond = second[s - 1];
                    var prev = result[f, s - 1];
                    var upper = result[f - 1, s];
                    var upperPrev = result[f - 1, s - 1];

                    if (currentFirst == currentSecond)
                    {
                        result[f, s] = upperPrev + 1;
                        continue;
                    }
                    result[f, s] = Math.Max(prev, upper);
                }
            }

            Console.WriteLine(result[result.GetLength(0) - 1, result.GetLength(1) - 1]);
        }
    }
}
