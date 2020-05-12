namespace CombinatorialAlgoritms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class Program
    {
        static void Main(string[] args)
        {
            RunTask08();
        }

        private static void RunTask08()
        {
            int n = int.Parse(Console.ReadLine());
            var res = GetSnakes(n);
            Console.WriteLine(string.Join(Environment.NewLine, res));
            Console.WriteLine($"Snakes count = {res.Count()}");
        }

        private static HashSet<string> AllCombs = new HashSet<string>();
        private static List<string> GetSnakes(int n)
        {

            var res = new List<string>();
            GenerateSnakeRoutes(n, new Stack<char>(new[] { 'S' }), res);
            return res;
        }

        private static HashSet<string> visitedCells = new HashSet<string>() { "0-0" };

        private static Dictionary<char, Func<int, int, int[]>> directions = new Dictionary<char, Func<int, int, int[]>>()
        {
            ['R'] = (int row, int col) => new[] { row, col + 1 },
            ['D'] = (int row, int col) => new[] { row + 1, col },
            ['L'] = (int row, int col) => new[] { row, col - 1 },
            ['U'] = (int row, int col) => new[] { row - 1, col },
        };
        public static void GenerateSnakeRoutes(int snakeLength, Stack<char> steps, List<string> routeBank, int row = 0, int col = 0)
        {
            if (steps.Count() == snakeLength)
            {
                string shape = string.Join("", steps.Reverse());
                if (!AlreadyEnlisted(routeBank, shape))
                {
                    routeBank.Add(shape);
                }
                return;
            }

            foreach (var dir in directions)
            {
                var tokens = dir.Value(row, col);
                char dirName = dir.Key;
                int rowNext = tokens[0];
                int colNext = tokens[1];
                var cell = $"{rowNext}-{colNext}";
                if (visitedCells.Contains(cell)) continue;

                visitedCells.Add(cell);
                steps.Push(dirName);

                GenerateSnakeRoutes(snakeLength, steps, routeBank, rowNext, colNext);

                visitedCells.Remove(cell);
                steps.Pop();
            }
        }

        private static bool AlreadyEnlisted(List<string> routeBank, string path)
        {
            if (AllCombs.Contains(path)) return true;

            var dirOrder = directions.Select(x => x.Key).ToArray();
            var p = path.ToCharArray();
           
            for (int i = 0; i < 4; i++)
            {
                AllCombs.Add(path);
                AllCombs.Add(XMirror(path));
                AllCombs.Add(YMirror(path));
                AllCombs.Add(Reversed(path));
                AllCombs.Add(XMirror(Reversed(path)));
                AllCombs.Add(YMirror(Reversed(path)));
                
                if (i == 3) break;

                for (int j = 1; j < p.Length; j++)
                {
                    var cur = p[j];
                    p[j] = dirOrder[(Array.IndexOf(dirOrder, cur) + 1) % dirOrder.Length];
                }

                path = string.Join("", p);
            }

            return false;
        }

        private static string XMirror(string path) => string.Join("", path.Select(x =>
                                                            {
                                                                if (x == 'U') return 'D';
                                                                if (x == 'D') return 'U';
                                                                return x;
                                                            }));

        private static string YMirror(string path) => string.Join("", path.Select(x =>
                                                            {
                                                                if (x == 'L') return 'R';
                                                                if (x == 'R') return 'L';
                                                                return x;
                                                            }));

        private static string Reversed(string path)
        {
            var p = path.ToCharArray();
            var temp = p[0];
            for (int i = 0; i < p.Length - 1; i++)
            {
                p[i] = p[i + 1];
            }
            p[p.Length - 1] = temp;
            Array.Reverse(p);
            return string.Join("", p);
        }
        private static void RunTask01_02()
        {
            var input = Console.ReadLine().Split(' ');
            var result = Parmutator.Permute(input).ToArray();
            Console.WriteLine(string.Join(Environment.NewLine, result.Select(x => string.Join(" ", x))));
        }

        private static void RunTask03()
        {
            var input = Console.ReadLine().Split(' ');
            int count = int.Parse(Console.ReadLine());
            var result = Parmutator.GetVariations(input, count).ToArray();
            Console.WriteLine(string.Join(Environment.NewLine, result.Select(x => string.Join(" ", x))));
        }

        private static void RunTask04()
        {
            var input = Console.ReadLine().Split(' ');
            int count = int.Parse(Console.ReadLine());
            var result = Parmutator.GetRepVariations(input, count).ToArray();
            Console.WriteLine(string.Join(Environment.NewLine, result.Select(x => string.Join(" ", x))));
        }

        private static void RunTask05()
        {
            var input = Console.ReadLine().Split(' ');
            int count = int.Parse(Console.ReadLine());
            var result = Parmutator.GetCombinations(input, count);
            Console.WriteLine(string.Join(Environment.NewLine, result.Select(x => string.Join(" ", x))));
        }

        private static void RunTask06()
        {
            var input = Console.ReadLine().Split(' ');
            int count = int.Parse(Console.ReadLine());
            var result = Parmutator.GetCombinationsRep(input, count);
            Console.WriteLine(string.Join(Environment.NewLine, result.Select(x => string.Join(" ", x))));
        }

        private static void RunTask07()
        {
            int n = int.Parse(Console.ReadLine());
            int k = int.Parse(Console.ReadLine());
            Console.WriteLine(Parmutator.GetKCombsFromN(n, k));
        }

        private class Parmutator
        {

            public static IList<T[]> Permute<T>(T[] input) where T : IComparable
            {
                var result = new List<T[]>();
                PermuteEngine(input.ToArray(), result);
                return result;
            }
            private static void PermuteEngine<T>(T[] arr, IList<T[]> bank, int start = 0) where T : IComparable
            {
                if (start >= arr.Length)
                {
                    bank.Add(arr.ToArray());
                    return;
                }

                PermuteEngine(arr, bank, start + 1);
                var used = new HashSet<T> { arr[start] };

                for (var i = start + 1; i < arr.Length; i++)
                {
                    if (!used.Contains(arr[i]))
                    {
                        used.Add(arr[i]);
                        SwapElements(arr, start, i);
                        PermuteEngine(arr, bank, start + 1);
                        SwapElements(arr, start, i);
                    }
                }
            }
            private static void SwapElements<T>(T[] input, int ind1, int ind2 = -1)
            {
                ind2 = ind2 == -1 ? ind1 + 1 : ind2;
                var temp = input[ind1];
                input[ind1] = input[ind2];
                input[ind2] = temp;
            }


            public static IList<ICollection<T>> GetVariations<T>(ICollection<T> input, int count) where T : IComparable
            {
                var result = new List<ICollection<T>>();
                var usedIndexes = new bool[input.Count()];
                VariationEngine(input.ToArray(), usedIndexes, new T[count], result);
                return result;
            }
            private static void VariationEngine<T>(T[] elmnts, bool[] usedIndexes, T[] currentComb, IList<ICollection<T>> bank, int index = 0)
            {
                if (index >= currentComb.Length) bank.Add(currentComb.ToArray());
                else
                {
                    for (int i = 0; i < elmnts.Length; i++)
                    {
                        if (!usedIndexes[i])
                        {
                            usedIndexes[i] = true;
                            currentComb[index] = elmnts[i];
                            VariationEngine(elmnts, usedIndexes, currentComb, bank, index + 1);
                            usedIndexes[i] = false;
                        }
                    }
                }
            }

            public static IList<ICollection<T>> GetRepVariations<T>(ICollection<T> input, int count) where T : IComparable
            {
                var result = new List<ICollection<T>>();

                VariationEngineRep(input.ToArray(), new T[count], result);
                return result;
            }
            private static void VariationEngineRep<T>(T[] elmnts, T[] currentComb, IList<ICollection<T>> bank, int index = 0)
            {
                if (index >= currentComb.Length) bank.Add(currentComb.ToArray());
                else
                {
                    for (int i = 0; i < elmnts.Length; i++)
                    {
                        currentComb[index] = elmnts[i];
                        VariationEngineRep(elmnts, currentComb, bank, index + 1);
                    }
                }
            }

            public static IList<ICollection<T>> GetCombinations<T>(ICollection<T> input, int count) where T : IComparable
            {
                var result = new List<ICollection<T>>();

                CombsEngine(input.ToArray(), new T[count], result);
                return result;
            }

            private static void CombsEngine<T>(T[] elmnts, T[] currentComb, IList<ICollection<T>> bank, int index = 0, int border = 0) where T : IComparable
            {
                if (index >= currentComb.Length) bank.Add(currentComb.ToArray());
                else
                {
                    for (int i = border; i < elmnts.Length; i++)
                    {
                        currentComb[index] = elmnts[i];
                        CombsEngine(elmnts, currentComb, bank, index + 1, i + 1);
                    }
                }
            }

            public static IList<ICollection<T>> GetCombinationsRep<T>(ICollection<T> input, int count) where T : IComparable
            {
                var result = new List<ICollection<T>>();

                CombsEngineRep(input.ToArray(), new T[count], result);
                return result;
            }

            private static void CombsEngineRep<T>(T[] elmnts, T[] currentComb, IList<ICollection<T>> bank, int index = 0, int border = 0) where T : IComparable
            {
                if (index >= currentComb.Length) bank.Add(currentComb.ToArray());
                else
                {
                    for (int i = border; i < elmnts.Length; i++)
                    {
                        currentComb[index] = elmnts[i];
                        CombsEngineRep(elmnts, currentComb, bank, index + 1, i);
                    }
                }
            }

            public static BigInteger GetKCombsFromN(int totalCount, int selectedCount) =>
                                     (Factoriel(totalCount)) / (Factoriel((totalCount - selectedCount)) * Factoriel(selectedCount));

            private static BigInteger Factoriel(int n)
            {
                BigInteger res = 1;
                for (int i = 2; i <= n; i++)
                {
                    res *= i;
                }

                return res;
            }
        }
    }
}