using System;
using System.Collections.Generic;

namespace LongestSubSequence
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] first = Console.ReadLine().ToCharArray();
            char[] second = Console.ReadLine().ToCharArray();

            int[,] salution = new int[first.Length + 1, second.Length + 1];

            for (int f = 1; f < salution.GetLength(0); f++)
            {
                var currentFirst = first[f - 1];
                for (int s = 1; s < salution.GetLength(1); s++)
                {
                    var currentSecond = second[s - 1];
                    var prev = salution[f, s - 1];
                    var upper = salution[f - 1, s];
                    var upperPrev = salution[f - 1, s - 1];

                    if (currentFirst == currentSecond)
                    {
                        salution[f, s] = upperPrev + 1;
                        continue;
                    }
                    salution[f, s] = Math.Max(prev, upper);
                }
            }

            Console.WriteLine(salution[salution.GetLength(0) - 1, salution.GetLength(1) - 1]);
            string sequence = GetSymbolsSequence(first, salution);

            Console.WriteLine(sequence);
        }

        private static string GetSymbolsSequence(char[] vertical, int[,] salution)
        {
            var symbols = new Stack<char>();

            int lastCol = salution.GetLength(1) - 1;
            for (int vc = salution.GetLength(0)-1; vc >= 1; vc--)
            {
                if (salution[vc, lastCol] > salution[vc - 1, lastCol])
                    symbols.Push(vertical[vc - 1]);
            }

            return string.Join("", symbols);
        }
    }
}
