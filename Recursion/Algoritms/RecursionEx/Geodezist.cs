using System;
using System.Collections.Generic;
using System.Linq;

namespace RecursionEx.Separated
{
    public class Geodezist
    {
        private readonly char visitedSymbol;
        private readonly char wallSymbol;
        private readonly char[] freeSymbols;

        public Geodezist() : this('c', '*')
        { }

        public Geodezist(char checkedSymbol, char blockSymbol, params char[] freeSymbols)
        {
            if (freeSymbols.Length == 0)
            {
                this.freeSymbols = new[] { '-', ' ' };
            }
            else
            {
                this.freeSymbols = freeSymbols;
            }
            this.visitedSymbol = checkedSymbol;
            this.wallSymbol = blockSymbol;
        }

        public IList<AreaInfo> MapThisMatrix(int rows)
        {
            char[][] board = PopulateBoard(rows);
            int reachedAreaId = 1;
            IList<AreaInfo> areasFd = new List<AreaInfo>();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < board[row].Length; col++)
                {
                    if (!IsFree(board, row, col) || board[row][col] == wallSymbol || areasFd.Any(x => x.Owns(row, col))) continue;
                    var locations = new HashSet<string>();
                    MeasureArea(locations, board, row, col);
                    areasFd.Add(new AreaInfo(reachedAreaId++, row, col, locations));
                }
            }

            return areasFd;
        }

        private void MeasureArea(HashSet<string> locations, char[][] board, int row, int col)
        {
            board[row][col] = visitedSymbol;

            locations.Add($"{row}|{col}");

            var direactions = GetAvalilableDirections(board, row, col);
            foreach (var dir in direactions)
            {
                MeasureArea(locations, board, dir[0], dir[1]);
            }

            board[row][col] = freeSymbols[0];
        }

        private char[][] PopulateBoard(int rows)
        {
            var board = new char[rows][];
            for (int i = 0; i < rows; i++)
            {
                board[i] = Console.ReadLine().Replace("\t", "").ToCharArray();
            }

            return board;
        }

        private IEnumerable<int[]> GetAvalilableDirections(char[][] board, int row, int col)
        {
            if (IsFree(board, row - 1, col)) yield return new[] { row - 1, col };
            if (IsFree(board, row, col + 1)) yield return new[] { row, col + 1 };
            if (IsFree(board, row + 1, col)) yield return new[] { row + 1, col };
            if (IsFree(board, row, col - 1)) yield return new[] { row, col - 1 };
        }

        private bool IsFree(char[][] board, int row, int col) =>
            Inside(board, row, col) && freeSymbols.Contains(board[row][col]) && !char.IsDigit(board[row][col]);

        private bool Inside(char[][] board, int row, int col) =>
            (row >= 0 && row < board.Length) && (col >= 0 && col < board[0].Length);
    }

    public class AreaInfo
    {
        private readonly HashSet<string> coords;

        public AreaInfo(int id, int startRow, int startCol, HashSet<string> coords)
        {
            Id = id;
            StartRow = startRow;
            StartCol = startCol;
            this.coords = coords;
        }

        public int Id { get; }
        public int StartRow { get; }
        public int StartCol { get; }
        public IList<int[]> Coordinates => coords.Select(x => x.Split('|').Select(int.Parse).ToArray()).ToList();
        public int Area => Coordinates.Count();
        public bool Owns(int row, int col) => coords.Contains($"{row}|{col}");
        public override string ToString() => $"Area #{Id} at ({StartRow}, {StartCol}), size: {Area}";
    }
}