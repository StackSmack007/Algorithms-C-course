namespace RecursionEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class Program
    {
        public static object[] Enumberable { get; private set; }

        static void Main(string[] args)
        {
            //  Task01();
            //  Task02();
            //  Task03();
            //  Task04();
            // Task05();
            Task06();
        }

        #region MeasureAreas        
        private static void Task06()
        {
            var geo = new Geodezist();
            int rows = int.Parse(Console.ReadLine());
            Console.ReadLine();
            var areas = geo.MapThisMatrix(rows);
            Console.WriteLine($"Total areas found: {areas.Count()}");
            int resultCounter = 1;
            foreach (var ar in areas.OrderByDescending(x => x.Area).ThenBy(x => x.StartRow).ThenBy(x => x.StartCol))
            {
                Console.WriteLine($"Area #{resultCounter++} at ({ar.StartRow}, {ar.StartCol}), size: {ar.Area}");
            }
        }

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
                IList<AreaInfo> areasFd = new List<AreaInfo>();
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < board[row].Length; col++)
                    {
                        if (!IsFree(board, row, col) || board[row][col] == wallSymbol || areasFd.Any(x => x.Owns(row, col))) continue;
                        var locations = new HashSet<string>();
                        MeasureArea(locations, board, row, col);
                        areasFd.Add(new AreaInfo(row, col, locations));
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

            public AreaInfo(int startRow, int startCol, HashSet<string> coords)
            {
                StartRow = startRow;
                StartCol = startCol;
                this.coords = coords;
            }

            public int StartRow { get; }
            public int StartCol { get; }
            public IList<int[]> Coordinates => coords.Select(x => x.Split('|').Select(int.Parse).ToArray()).ToList();
            public int Area => Coordinates.Count();
            public bool Owns(int row, int col) => coords.Contains($"{row}|{col}");
        }
        #endregion

        private static void Task05()
        {
            int range = int.Parse(Console.ReadLine());
            int comboLength = int.Parse(Console.ReadLine());
            PrintNumCombos(range, new int[comboLength], false);
        }

        #region HanoiTowers
        private static void Task04()
        {
            var range = Enumerable.Range(1, int.Parse(Console.ReadLine()));
            var source = new Stack<int>(range.Reverse());
            var destination = new Stack<int>();
            var spare = new Stack<int>();
            var all = new[] { source, destination, spare };
            PrintStatus(all);
            stepCounter = 0;
            MoveDisks(range.Count(), source, destination, spare, all);
        }
        public static void PrintStatus(Stack<int>[] all)
        {
            Console.WriteLine($"Source: {string.Join(", ", all[0].Reverse())}");
            Console.WriteLine($"Destination: {string.Join(", ", all[1].Reverse())}");
            Console.WriteLine($"Spare: {string.Join(", ", all[2].Reverse())}");
            Console.WriteLine();
        }
        private static int stepCounter = 0;
        public static void MoveDisks(int n, Stack<int> source, Stack<int> destination, Stack<int> spare, Stack<int>[] all)
        {
            if (n > 0)
            {
                MoveDisks(n - 1, source, spare, destination, all);
                destination.Push(source.Pop());
                Console.WriteLine($"Step #{++stepCounter}: Moved disk");//: {destination.Peek()}");
                PrintStatus(all);
                MoveDisks(n - 1, spare, destination, source, all);
            }
        }
        #endregion

        #region 2.Nested Loops To Recursion
        private static void Task03()
        {
            int range = int.Parse(Console.ReadLine());
            int comboLength = int.Parse(Console.ReadLine());
            PrintNumCombos(range, new int[comboLength]);
        }
        #endregion

        #region 2.Nested Loops To Recursion
        private static void Task02()
        {
            int num = int.Parse(Console.ReadLine());
            PrintNumCombos(num, new int[num]);
        }

        private static void PrintNumCombos(int num, int[] result, bool repeat = true, int index = 0, int border = 1)
        {
            if (index == result.Length)
            {
                Console.WriteLine(string.Join(" ", result));
                return;
            }
            for (int i = border; i <= num; i++)//values
            {
                result[index] = i;
                if (repeat) PrintNumCombos(num, result, repeat, index + 1);
                else PrintNumCombos(num, result, repeat, index + 1, i + 1);
            }
        }
        #endregion

        #region ReverseArray
        public static void Task01()
        {
            var array = Console.ReadLine().Split(' ');
            PrintReversedArray(array);
        }
        public static void PrintReversedArray(string[] arr)
        {
            var result = GetReversedArray(arr, new string[arr.Length], arr.Length);
            Console.WriteLine(string.Join(" ", result));
        }
        public static string[] GetReversedArray(string[] arr, string[] result, int index)
        {
            if (index == 0) return result;
            result[result.Length - index] = arr[--index];
            return GetReversedArray(arr, result, index);
        }
        #endregion
    }
}