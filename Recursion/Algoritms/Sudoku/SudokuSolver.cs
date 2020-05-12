namespace Sudoku
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class SudokuSolver
    {
        private const int SIZE = 9;

        public int[][] SetBoard(string delimiter = "")
        {
            var board = new int[SIZE][];
            for (int i = 0; i < SIZE; i++)
            {
                if (string.IsNullOrEmpty(delimiter)) board[i] = Console.ReadLine().ToCharArray().Select(x => x - '0').ToArray();
                else board[i] = Console.ReadLine().Split(delimiter).Select(int.Parse).ToArray();
                if (board[i].Length != SIZE) throw new ArgumentOutOfRangeException("Invalid row!");
                if (board[i].Any(x => x > SIZE || x < 0)) throw new ArgumentOutOfRangeException("Invalid cell value!");
            }

            return board;
        }

        public int[][] ReadBoardFromFile(string path = "../../../Tests.txt", string delimiter = " ")
        {
            string text = System.IO.File.ReadAllText(path);
            var delimiters = new[] { "\t", " ", delimiter };
            var board = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Split(delimiters,StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();
            if (board.Length != SIZE) throw new ArgumentOutOfRangeException("Invalid row Count!");
            if (board.Any(x => x.Length != SIZE)) throw new ArgumentOutOfRangeException("Invalid element count in row!");
            if (board.SelectMany(x => x).Any(x => x < 0 || x > SIZE)) throw new ArgumentOutOfRangeException("Invalid value of cell");
            return board;
        }

        public void Print(int[][] result, string delimiter = " ")
        {
            foreach (var row in result)
            {
                Console.WriteLine(string.Join(delimiter, row));
            }
        }

        public IList<int[][]> Solve(int[][] board)
        {
            var b = board.Select(x => x.ToArray()).ToArray();
            var result = new List<int[][]>();
            FindSalutionRec(b, result);
            return result;
        }

        private void FindSalutionRec(int[][] salution, List<int[][]> salutionBank, int row = 0, int col = 0)
        {
            if (AllFilled(salution))
            {
                salutionBank.Add(salution.Select(x => x.ToArray()).ToArray());
                return;
            }
            if (row == -1) return;

            var nextCell = GetNextCoordinates(salution, row, col);

            //Can't touch this...go forward
            if (salution[row][col] != 0)
            {
                FindSalutionRec(salution, salutionBank, nextCell.Key, nextCell.Value);
                return;
            }

            for (int val = 1; val <= SIZE; val++)
            {
                if (!TryWriteValue(salution, row, col, val)) continue;
                FindSalutionRec(salution, salutionBank, nextCell.Key, nextCell.Value);
            }

            salution[row][col] = default(int);
        }

        private KeyValuePair<int, int> GetNextCoordinates(int[][] board, int curRow, int curCol)
        {
            for (int i = curRow; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (i == curRow && j <= curCol) continue; //we are before our symbol in the same row

                    if (board[i][j] == default(int))
                    {
                        return new KeyValuePair<int, int>(i, j);
                    }
                }
            }

            return new KeyValuePair<int, int>(-1, -1);
        }

        private bool TryWriteValue(int[][] current, int row, int col, int val)
        {
            bool usedInRow = current[row].Contains(val);
            bool usedInCol = current.Select(x => x[col]).Contains(val);
            if (usedInRow || usedInCol) return false;

            #region isUsedInSector3x3
            int startRow = 3 * (row / 3);
            int startCol = 3 * (col / 3);
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    if (current[i][j] == val) return false;
                }
            }
            #endregion

            current[row][col] = val;
            return true;
        }

        private bool AllFilled(int[][] board) => !board.SelectMany(x => x).Contains(default(int));
    }
}
