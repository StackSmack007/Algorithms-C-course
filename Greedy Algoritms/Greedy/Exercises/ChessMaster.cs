using System.Collections.Generic;
using System.Linq;

namespace Exercises
{
    public class ChessMaster
    {
        private int[][] board;
        public int[][] GetMovesKnight(int size, int row = 0, int col = 0)
        {
            this.board = MakeBoard(size);

            int counter = 0;
            Location nextMove = new Location(row, col);

            do
            {
                row = nextMove.Row;
                col = nextMove.Col;
                board[row][col] = ++counter;
                nextMove = GetRoutes(row, col).FirstOrDefault();
            }
            while (nextMove != null);

            return this.board;
        }

        private int[][] MakeBoard(int size)
        {
            var board = new int[size][];
            for (int i = 0; i < size; i++)
            {
                board[i] = new int[size];
            }

            return board;
        }

        private int[] GetNextStep()
        {
            var coords = new int[2];

            return coords;
        }

        private ICollection<Location> GetRoutes(int baseRow, int baseCol)
        {
            return GetDirections(baseRow, baseCol)
                .Select(x => new Location
                (
                    x[0],
                    x[1],
                    GetDirections(x[0], x[1]).Count()
                ))
                .OrderBy(x => x.Power)
                .ToArray();
        }

        private int[][] GetDirections(int row, int col)
        {
            return new[]
            {
                new []{row+1,col+2},
                new []{row-1,col+2},
                new []{row+1,col-2},
                new []{row-1,col-2},
                new []{row+2,col+1},
                new []{row+2,col-1},
                new []{row-2,col+1},
                new []{row-2,col-1},
            }.Where(x => IsInBoard(x[0], x[1]) && IsFree(x[0], x[1])).ToArray();
        }
        private bool IsFree(int row, int col) => this.board[row][col] == 0;
        private bool IsInBoard(int row, int col) =>
                row >= 0 && col >= 0 && row < board.Length && col < board.Length;

        private class Location
        {
            public Location(int row, int col, int power = -1)
            {
                Row = row;
                Col = col;
                Power = power;
            }

            public int Row { get; }
            public int Col { get; }
            public int Power { get; }
        }
    }

}