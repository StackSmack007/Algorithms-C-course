using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_02
{
    class Program
    {
        static void Main(string[] args)
        {
            var cells = ReadCells();
            Dictionary<char, int> roots = GetZonesCounts(cells);
            PrintResult(roots);
        }
        private static List<Cell> ReadCells()
        {
            int rows = int.Parse(Console.ReadLine());
            var cells = new List<Cell>();
            for (int row = 0; row < rows; row++)
            {
                var input = Console.ReadLine();

                for (int i = 0; i < input.Length; i++)
                {
                    cells.Add(new Cell(row, i, input[i]));
                }
            }
            return cells;
        }
        private static Dictionary<char, int> GetZonesCounts(List<Cell> cells)
        {
            Dictionary<Cell, List<Cell>> graph = MakeGraph(cells);

            Cell[] rootCells = KruskalRoots(graph);

            var result = rootCells.GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.Count());
            return result;
        }
        private static Dictionary<Cell, List<Cell>> MakeGraph(List<Cell> cells)
        {
            var graph = new Dictionary<Cell, List<Cell>>();
            foreach (var cell in cells)
            {
                var edges = new[]
               {
                    cells.FirstOrDefault(x=>x.Row==cell.Row && x.Col==cell.Col+1 && x.Value==cell.Value),
                    cells.FirstOrDefault(x=>x.Row==cell.Row && x.Col==cell.Col-1 && x.Value==cell.Value),
                    cells.FirstOrDefault(x=>x.Row==cell.Row+1 && x.Col==cell.Col && x.Value==cell.Value),
                    cells.FirstOrDefault(x=>x.Row==cell.Row-1 && x.Col==cell.Col && x.Value==cell.Value),
                }.Where(x => x != null).ToList();
                graph[cell] = edges;
            }

            return graph;
        }
        private static Cell[] KruskalRoots(Dictionary<Cell, List<Cell>> graph)
        {
            Dictionary<string, string> parents = graph.Keys.ToDictionary(x => x.Id, x => x.Id);
            foreach (var edge in graph)
            {
                foreach (var target in edge.Value)
                {
                    string firstRoot = GetRoot(parents, edge.Key.Id);
                    string secondRoot = GetRoot(parents, target.Id);
                    if (firstRoot == secondRoot)
                        continue;
                    parents[secondRoot] = edge.Key.Id;
                }
            }

            return graph.Keys.Where(x => parents[x.Id] == x.Id).ToArray();
        }
        private static string GetRoot(Dictionary<string, string> parents, string id)
        {
            while (parents[id] != id)
                id = parents[id];
            return id;
        }
        private static void PrintResult(Dictionary<char, int> roots)
        {
            Console.WriteLine($"Areas: {roots.Values.Sum()}");
            foreach (var kvp in roots.OrderBy(x => x.Key))
            {
                Console.WriteLine($"Letter '{kvp.Key}' -> {kvp.Value}");
            }
        }
        private class Cell
        {
            public Cell(int row, int col, char value)
            {
                Row = row;
                Col = col;
                Value = value;
            }

            public int Row { get; }
            public int Col { get; }
            public char Value { get; }

            public string Id => $"{Row}|{Col}";

        }
    }
}