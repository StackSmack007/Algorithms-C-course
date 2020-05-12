namespace Sudoku
{
    using System;
    using System.Linq;
    class Program
    {
        static void Main(string[] args)
        {
            var boardMaster = new SudokuSolver();
           
            //  var board = boardMaster.SetBoard();
            var board = boardMaster.ReadBoardFromFile();
           
            var result = boardMaster.Solve(board);

            foreach (var salution in result)
            {
                boardMaster.Print(salution);
                Console.WriteLine(new string('-', 17));
            }

            if (result.Count() > 1) Console.WriteLine("Salutions count: " + result.Count());
            else if (result.Count() == 0) Console.WriteLine("No solution possible!");
        }
    }
}
