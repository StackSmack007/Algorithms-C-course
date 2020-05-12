namespace Recursion.Complicated
{
    using System.Collections.Generic;
    using System.Text;

    public class PathFinder
    {
        private const char visSymbol = 'v';
        private readonly char exitSymbol;
        private readonly char freeSymbol;
        private readonly char blockSymbol;

        public PathFinder() : this('e', '_', '*')
        { }
        public PathFinder(char exitSymbol = 'e', char freeSymbol = '_', char blockSymbol = '*')
        {
            this.exitSymbol = exitSymbol;
            this.freeSymbol = freeSymbol;
            this.blockSymbol = blockSymbol;
        }
        private char[][] MakeBoard(IList<string> rows)
        {
            var board = new char[rows.Count][];
            for (int i = 0; i < rows.Count; i++)
            {
                board[i] = rows[i].ToCharArray();
            }
            return board;
        }

        public ICollection<string> GetPaths(IList<string> rows, int entryRow = 0, int entryCol = 0)
        {
            char[][] board = MakeBoard(rows);
            IList<string> pathsFound = new List<string>();
            if (Inside(board, entryRow, entryCol)) FindPaths(pathsFound, board, entryRow, entryCol, new StringBuilder());
            return pathsFound;
        }

        private void FindPaths(IList<string> pathsFound, char[][] board, int row, int col, StringBuilder pathCurrent)
        {
            if (board[row][col] == exitSymbol)
            {
                pathsFound.Add(pathCurrent.ToString());
                return;
            }

            board[row][col] = visSymbol;
            var directions = GetAvalilableDirections(board, row, col);
            foreach (var kvp in directions)
            {
                pathCurrent.Append(kvp.Key);
                FindPaths(pathsFound, board, kvp.Value.Row, kvp.Value.Col, pathCurrent);
                pathCurrent.Remove(pathCurrent.Length - 1, 1);
            }

            board[row][col] = freeSymbol;
        }

        private IDictionary<char, Location> GetAvalilableDirections(char[][] board, int row, int col)
        {
            var routes = new Dictionary<char, Location>();
            if (IsFree(board, row - 1, col)) routes['U'] = new Location(row - 1, col);
            if (IsFree(board, row, col + 1)) routes['R'] = new Location(row, col + 1);
            if (IsFree(board, row + 1, col)) routes['D'] = new Location(row + 1, col);
            if (IsFree(board, row, col - 1)) routes['L'] = new Location(row, col - 1);
            return routes;
        }

        private bool IsFree(char[][] board, int row, int col) =>
            Inside(board, row, col) && board[row][col] != blockSymbol && board[row][col] != visSymbol;

        private bool Inside(char[][] board, int row, int col) =>
            (row >= 0 && row < board.Length) && (col >= 0 && col < board[0].Length);

        private class Location
        {
            public Location(int row, int col)
            {
                Row = row;
                Col = col;
            }
            public int Row { get; }
            public int Col { get; }
        }
    }
}