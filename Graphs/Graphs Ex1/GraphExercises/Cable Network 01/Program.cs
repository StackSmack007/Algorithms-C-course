using System;
using System.Collections.Generic;
using System.Linq;

namespace Cable_Network_01
{
    public static class Program
    {
        private static Dictionary<int, int> parents = new Dictionary<int, int>();
        static void Main(string[] args)
        {
            int budget = int.Parse(Console.ReadLine().Split(' ')[1]);
            int nodes = int.Parse(Console.ReadLine().Split(' ')[1]);
            for (int i = 0; i < nodes; i++)
                parents[i] = i;

            int edgeCount = int.Parse(Console.ReadLine().Split(' ')[1]);

            List<Edge> edges = ReadEdges(edgeCount);
            PlaceExistingConnections(edges.Where(x => x.IsUsed).ToArray());

            ICollection<Edge> edgesToPlace = PickOptimalEdges(budget, edges.Where(x => !x.IsUsed).OrderBy(x => x.Cost).ToArray());
            Console.WriteLine($"Budget used: {edgesToPlace.Sum(x => x.Cost)}");
        }

        private static List<Edge> ReadEdges(int edgeCount)
        {
            var result = new List<Edge>(edgeCount);
            for (int i = 0; i < edgeCount; i++)
            {
                var input = Console.ReadLine().Split(' ');
                var tokens = input.Take(3).Select(int.Parse).ToArray();
                bool isConnected = input.Length > 3;
                result.Add(new Edge(tokens[0], tokens[1], tokens[2], isConnected));
            }
            return result;
        }
        private static void PlaceExistingConnections(Edge[] edges)
        {
            foreach (var edge in edges)
            {
                int firstRoot = GetRoot(edge.First);
                int secondRoot = GetRoot(edge.Second);
                if (firstRoot != secondRoot)
                    parents[secondRoot] = edge.First;
            }
        }

        private static int GetRoot(int id)
        {
            while (id != parents[id])
                id = parents[id];
            return id;
        }
        private static ICollection<Edge> PickOptimalEdges(int budget, Edge[] edges)
        {
            var rootesWithChildren = parents.Where(x => x.Key != x.Value).Select(x => GetRoot(x.Value)).Distinct();
            var result = new List<Edge>();

            var edgePool = edges.Where(x => x.Cost <= budget).ToList();

            int soloPairs = 0;
            while (edgePool.Count > 0)
            {
                var edge = edgePool.First();
                edgePool.Remove(edge);

                int firstRoot = GetRoot(edge.First);
                int secondRoot = GetRoot(edge.Second);

                if (firstRoot == secondRoot)
                    continue;

                bool connectsNodeToEstablishedNetwork = !rootesWithChildren.Any() ||
                    rootesWithChildren.Contains(firstRoot) || rootesWithChildren.Contains(secondRoot);

                if (!connectsNodeToEstablishedNetwork)
                {
                    if (++soloPairs < edgePool.Count)
                        edgePool.Add(edge);

                    continue;
                }

                parents[secondRoot] = edge.First;
                budget -= edge.Cost;
                edgePool = edgePool.Where(x => x.Cost <= budget).OrderBy(x => x.Cost).ToList();
                result.Add(edge);
            }

            return result;
        }
    }

    internal class Edge
    {
        public Edge(int first, int second, int cost, bool isUsed = false)
        {
            this.First = first;
            this.Second = second;
            this.Cost = cost;
            this.IsUsed = isUsed;
        }

        public int First { get; }
        public int Second { get; }
        public int Cost { get; }
        public bool IsUsed { get; set; }
    }
}