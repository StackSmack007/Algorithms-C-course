using System;
using System.Collections.Generic;
using System.Numerics;

namespace Fibbonacci
{
    class Program
    {
        private static Dictionary<int, BigInteger> results = new Dictionary<int, BigInteger>();
        static void Main(string[] args)
        {
                int n = int.Parse(Console.ReadLine());
                Console.WriteLine(Fib(n));
        }
        private static BigInteger Fib(int n)
        {
            if (!results.ContainsKey(1) || !results.ContainsKey(2))
            {
                results[1] = 1;
                results[2] = 1;
            }
            if (!results.ContainsKey(n))
            {
                results[n] = Fib(n - 1) + Fib(n - 2);
            }

            return results[n];
        }
    }
}
