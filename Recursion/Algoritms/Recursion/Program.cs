namespace Recursion
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;

    public class Program
    {
        static void Main(string[] args)
        {
            // Task01();
            // Task02();
            // Task03();
            // Task04();
            // Task05();
            // Task06();
            Task07();
        }

        #region GenerateCombinations    
        public static void Task05()
        {
            int[] set = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            int optCount = int.Parse(Console.ReadLine());
            var result = GenerateCombinations(optCount, set);
            foreach (var arr in result)
            {
                Console.WriteLine(string.Join(" ", arr));
            }
        }

        public static IEnumerable<T[]> GenerateCombinations<T>(int optCount, T[] optSet)
        {
            var resultSet = new List<T[]>();
            PopulateCombs(resultSet, new T[optCount], optSet);
            return resultSet;
        }

        private static void PopulateCombs<T>(IList<T[]> dataBank, T[] res, T[] set, int index = 0, int border = 0)
        {
            if (index >= res.Length)
            {
                dataBank.Add(res.ToArray());
                return;
            }

            for (int i = border; i < set.Length; i++)
            {
                res[index] = set[i];
                PopulateCombs(dataBank, res, set, index + 1, i + 1);
            }
        }
        #endregion

        #region GenerateVectors    
        public static void Task04()
        {
            int optCount = int.Parse(Console.ReadLine());
            var set = new[] { 0, 1 };
            var result = GenerateVectors(optCount, set);
            foreach (var arr in result)
            {
                Console.WriteLine(string.Join("", arr));
            }
        }

        public static IEnumerable<T[]> GenerateVectors<T>(int optCount, T[] optSet)
        {
            var resultSet = new List<T[]>();
            PopulateVectors(resultSet, new T[optCount], optSet);
            return resultSet;
        }

        private static void PopulateVectors<T>(IList<T[]> dataBank, T[] res, T[] set, int index = 0)
        {
            if (index == res.Count())
            {
                dataBank.Add(res.ToArray());
                return;
            }

            for (int i = 0; i < set.Count(); i++)
            {
                res[index] = set[i];
                PopulateVectors(dataBank, res, set, index + 1);
            }
        }

        #endregion

        #region PictureDraw    
        public static void Task03()
        {
            int number = int.Parse(Console.ReadLine());
            Draw(number);
        }

        private static void Draw(int cnt, char startSymbol = '*', char endSymbol = '#')
        {
            if (cnt == 0) return;
            Console.WriteLine(new string(startSymbol, cnt));
            Draw(cnt - 1);
            Console.WriteLine(new string(endSymbol, cnt));
        }
        #endregion

        #region Factoriel    
        public static void Task02()
        {
            int number = int.Parse(Console.ReadLine());
            Console.WriteLine(RecFactoriel(number));
        }

        private static int RecFactoriel(int number)
        {
            if (number == 1) return 1;
            return number * RecFactoriel(number - 1);
        }
        #endregion

        #region ArraySum 
        public static void Task01()
        {
            int[] numbers = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            Console.WriteLine(Sum(numbers));
        }

        public static int Sum(int[] arr, int index = 0)
        {
            if (index == arr.Length) return 0;
            return arr[index] + Sum(arr, index + 1);
        }
        #endregion

        #region 8Queens
        private static void Task06()
        {
            var queenMaster = new QueensMaster(8);

            var salutions = queenMaster.GenerateQueenCombos(8);
            string[] results = salutions.Select(QueensMaster.StringifySalution).ToArray();
            var result = string.Join(Environment.NewLine, results);
            Console.WriteLine(result);
            Console.WriteLine(results.Count());

        }
        public class QueensMaster
        {
            public QueensMaster(int boardSize, char freeSymbol = '-', char queenSymbol = '*')
            {
                BoardSize = boardSize;
                FreeSymbol = freeSymbol;
                QueenSymbol = queenSymbol;
            }
            public int BoardSize { get; }
            public char FreeSymbol { get; }
            public char QueenSymbol { get; }

            public ICollection<char[][]> GenerateQueenCombos(int queenCount)
            {
                var board = new char[BoardSize][];
                SetBoardClear(board);
                IList<char[][]> salutions = new List<char[][]>();

                if (queenCount <= BoardSize) FindSalutions(salutions, board, queenCount);
                return salutions;
            }

            private void FindSalutions(IList<char[][]> salutions, char[][] board, int queenCount, int row = 0)
            {

                int rowInterval = QueenCountUsed(board) == queenCount - 1 ? BoardSize : row + 1;
                for (int i = row; i < rowInterval; i++)
                {
                    for (int j = 0; j < BoardSize; j++)
                    {
                        if (!TryQueenPlace(board, i, j)) continue;
                        if (queenCount == QueenCountUsed(board))
                        {
                            salutions.Add(board.Select(x => x.ToArray()).ToArray());
                            board[i][j] = FreeSymbol;
                            continue;
                        }
                        if (row == j && j == BoardSize - 1) continue;
                        FindSalutions(salutions, board, queenCount, i + 1);
                        board[i][j] = FreeSymbol;
                    }
                }
            }

            public static string StringifySalution(char[][] board)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var arr in board)
                {
                    sb.AppendLine(string.Join(" ", arr));
                }
                return sb.ToString();
            }

            private void SetBoardClear(char[][] board)
            {
                for (int i = 0; i < board.Length; i++)
                {
                    board[i] = (new string(FreeSymbol, BoardSize)).ToCharArray();
                }
            }

            private int QueenCountUsed(char[][] sal)

            {
                var res = sal.Select(x => x.Where(r => r == QueenSymbol).Count()).Sum();
                return res;
            }

            private int[] GetNextCoords(int row, int col)
            {
                if (col < BoardSize - 1) return new[] { row, col + 1 };
                else return new[] { row + 1, 0 };
            }

            private bool CordsInBoard(int row, int col) => row >= 0 && row < BoardSize && col >= 0 && col < BoardSize;
            private bool TryQueenPlace(char[][] board, int row, int col)
            {
                bool verticalAttack = board.Any(x => x[col] == QueenSymbol);
                bool diagonalAttack = false;

                int[] leftTopCornerPoint = new[] { row - Math.Min(row, col), col - Math.Min(row, col) };
                int[] rightTopCornerPoint = new[] { row - Math.Min(BoardSize - col - 1, row), col + Math.Min(BoardSize - col - 1, row) };

                while (CordsInBoard(leftTopCornerPoint[0], leftTopCornerPoint[1]) && !diagonalAttack)
                {
                    if (board[leftTopCornerPoint[0]][leftTopCornerPoint[1]] == QueenSymbol)
                    {
                        diagonalAttack = true;
                    }
                    leftTopCornerPoint[0]++;
                    leftTopCornerPoint[1]++;
                }

                while (CordsInBoard(rightTopCornerPoint[0], rightTopCornerPoint[1]) && !diagonalAttack)
                {
                    if (board[rightTopCornerPoint[0]][rightTopCornerPoint[1]] == QueenSymbol)
                    {
                        diagonalAttack = true;
                    }
                    rightTopCornerPoint[0]++;
                    rightTopCornerPoint[1]--;
                }

                if (verticalAttack || diagonalAttack) return false;
                board[row][col] = QueenSymbol;
                return true;
            }
        }
        #endregion


        #region FindAllPaths
        private static void Task07()
        {
            int rowCount = int.Parse(Console.ReadLine());
            int.Parse(Console.ReadLine());
            IList<string> rows = new List<string>(rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                rows.Add(Console.ReadLine());
            }

            var pathFinder = new PathFinder();
            var paths = pathFinder.GetPaths(rows).OrderBy(x => x.Length);
            Console.WriteLine(string.Join(Environment.NewLine, paths));
        }

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
        #endregion
    }
}