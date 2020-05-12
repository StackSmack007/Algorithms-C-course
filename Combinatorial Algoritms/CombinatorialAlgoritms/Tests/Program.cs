using System;
using System.Collections.Generic;
using System.Linq;

internal class Program
{
    static char[] permutation;
    private static void Main()
    {
        permutation = Console.ReadLine()
            .Split()
            .Select(Char.Parse)
            .ToArray();

        PermuteRep(0);
    }



    private static void Swap(int i, int j)
    {
        var temp = permutation[i];
        permutation[i] = permutation[j];
        permutation[j] = temp;
    }
}