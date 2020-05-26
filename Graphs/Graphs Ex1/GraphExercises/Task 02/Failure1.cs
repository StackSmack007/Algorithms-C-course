//using System;
//using System.Collections.Generic;

//namespace Task_02
//{
//    class Program
//    {
//        private static char[][] board;
//        private static bool[][] modified;
//        private static int[] parents;
//        static void Main(string[] args)
//        {
//            ReadInput();


//            MakeConnections();
//            List<char> uniques = new List<char>();

//            for (int i = 0; i < parents.Length; i++)
//            {
//                if (i != parents[i])
//                    continue;

//                var coordinates = GetCoordsFromId(parents[i]);
//                uniques.Add(board[coordinates[0]][coordinates[1]]);
//            }
//        }

//        private static void MakeConnections()
//        {
//            for (int row = 0; row < board.Length; row++)
//            {
//                for (int col = 0; col < board[row].Length; col++)
//                {
//                    int firstId = GetIdFromCoords(row, col);
//                    int secondId = 0;

//                    if (col + 1 < board[row].Length && board[row][col] == board[row][col + 1] && !modified[row][col + 1])
//                    {
//                        modified[row][col] = true;
//                        secondId = GetIdFromCoords(row, col + 1);
//                        modified[row][col + 1] = true;
//                        parents[secondId] = firstId;
//                    }

//                    if (col > 0 && board[row][col] == board[row][col - 1] && !modified[row][col - 1])
//                    {
//                        modified[row][col] = true;
//                        secondId = GetIdFromCoords(row, col - 1);
//                        modified[row][col - 1] = true;
//                        parents[secondId] = firstId;
//                    }

//                    if (row + 1 < board.Length && board[row][col] == board[row + 1][col] && !modified[row + 1][col])
//                    {
//                        modified[row][col] = true;
//                        secondId = GetIdFromCoords(row + 1, col);
//                        modified[row + 1][col] = true;
//                        parents[secondId] = firstId;
//                    }

//                    if (row > 0 && board[row][col] == board[row - 1][col] && !modified[row - 1][col])
//                    {
//                        modified[row][col] = true;
//                        secondId = GetIdFromCoords(row - 1, col);
//                        modified[row - 1][col] = true;
//                        parents[secondId] = firstId;
//                    }
//                }
//            }
//        }
//        private static int GetIdFromCoords(int row, int col)
//        {
//            int id = col;
//            for (int i = 0; i < row; i++)
//            {
//                id += board[i].Length;
//            }
//            return id;
//        }

//        private static int[] GetCoordsFromId(int id)
//        {
//            int row = 0;
//            int col = id;
//            while (col >= board[row].Length)
//            {
//                row++;
//                col -= board[row].Length;
//            }

//            int[] rowCol = new[] { row, col };
//            return rowCol;
//        }

//        private static void ReadInput()
//        {
//            int rows = int.Parse(Console.ReadLine());
//            board = new char[rows][];
//            modified = new bool[rows][];
//            int elCount = 0;
//            for (int i = 0; i < rows; i++)
//            {

//                board[i] = Console.ReadLine().ToCharArray();
//                modified[i] = new bool[board[i].Length];
//                elCount += board[i].Length;
//            }

//            parents = new int[elCount];
//            for (int i = 1; i < parents.Length; i++)
//                parents[i] = i;
//        }
//    }
//}
