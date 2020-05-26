using System;
using System.Collections.Generic;
using System.Linq;

namespace Modified_Kruskal
{
    class Program
    {
        static void Main(string[] args)
        {
               int nodeCount = int.Parse(Console.ReadLine().Split(' ')[1]);
            int edgeCount = int.Parse(Console.ReadLine().Split(' ')[1]);
            var edges = ReadEdges(edgeCount);
            var requiredEdges = Kruskal(edges);
            PrintResult(requiredEdges);
        }

        private static List<Edge> ReadEdges(int edgeCount)
        {
            var result = new List<Edge>(edgeCount);
            for (int i = 0; i < edgeCount; i++)
            {
                var tokens = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                result.Add(new Edge(tokens[0], tokens[1], tokens[2]));
            }

            return result;
        }

        private static Dictionary<int, int> parents;
        private static IList<Edge> Kruskal(ICollection<Edge> edges)
        {
            parents = edges.Select(x => x.First).Union(edges.Select(x => x.Second)).Distinct().ToDictionary(x => x, x => x);
            var result = new List<Edge>();

            foreach (var edge in edges.OrderBy(x => x.Weight))
            {
                int firstRoot = GetRoot(edge.First);
                int secondRoot = GetRoot(edge.Second);
                if (firstRoot != secondRoot)
                {
                    parents[secondRoot] = edge.First;
                    result.Add(edge);
                }
            }

            return result;
        }

        private static int GetRoot(int id)
        {
            while (id != parents[id])
                id = parents[id];
            return id;
        }

        private static void PrintResult(IList<Edge> requiredEdges)
        {
            Console.WriteLine($"Minimum spanning forest weight: {requiredEdges.Sum(x => x.Weight)}");
          //  Console.WriteLine(string.Join(Environment.NewLine, requiredEdges.Select(x => x.ToString())));
        }
    }

    internal class Edge
    {
        public Edge(int first, int second, int weight)
        {
            this.First = first;
            this.Second = second;
            this.Weight = weight;

        }

        public int First { get; }
        public int Second { get; }
        public int Weight { get; }

        public override string ToString() =>
            $"({First} {Second}) -> {Weight}";

    }
}
