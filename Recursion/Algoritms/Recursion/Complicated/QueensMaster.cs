namespace Recursion.Complicated
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
}