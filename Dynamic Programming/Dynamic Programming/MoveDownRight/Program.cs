using System;
using System.Collections.Generic;
using System.Linq;

namespace MoveDownRight
{
    class Program
    {
        static void Main(string[] args)
        {
            int rows = int.Parse(Console.ReadLine());
            Console.ReadLine();
            var board = new int[rows][];
            var salution = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                board[i] = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                salution[i] = new int[board[i].Length];
            }

            for (int row = 0; row < board.Length; row++)
            {
                for (int col = 0; col < board[row].Length; col++)
                {
                    if (row == 0 && col == 0) salution[row][col] = board[row][col];
                    if (row > 0 && col > 0) salution[row][col] = Math.Max(salution[row - 1][col], salution[row][col - 1]) + board[row][col];
                    if (row == 0 && col > 0) salution[row][col] = salution[row][col - 1] + board[row][col];
                    if (row > 0 && col == 0) salution[row][col] = salution[row - 1][col] + board[row][col];
                }
            }
            var result = GetPath(salution);
            Console.WriteLine(string.Join(" ", result));
        }

        private static IReadOnlyCollection<string> GetPath(int[][] sal)
        {
            var path = new Stack<string>();
            var celFormat = "[{0}, {1}]";

            int row = sal.Length - 1;
            int col = sal[row].Length - 1;

            while (true)
            {
                path.Push(string.Format(celFormat, row, col));
                if (row == 0 && col == 0) break;

                if (row == 0) col--;
                else if (col == 0) row--;
                else if (sal[row][col - 1] >= sal[row - 1][col]) col--;
                else row--;
            }

            return path;
        }

    }
}
