using System;
using System.Collections.Generic;
using System.Linq;

namespace Safest_Path_03
{
    class Program
    {
        static void Main(string[] args)
        {
            int nodes = int.Parse(Console.ReadLine().Split(' ')[1]);
            int[] nodesStartEnd = Console.ReadLine().Split(new[] { " ", "–", "-" }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(int.Parse)
                .ToArray();

            int edgesCount = int.Parse(Console.ReadLine().Split(' ')[1]);
            List<Edge> graph = ReadEdges(edgesCount);

            var path = GetRoutes(graph, nodesStartEnd[0], nodesStartEnd[1]);
            PrintResults(path);
        }

        private static List<Edge> ReadEdges(int edgesCount)
        {
            var edges = new List<Edge>();
            for (int i = 0; i < edgesCount; i++)
            {
                int[] tokens = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                edges.Add(new Edge(tokens[0], tokens[1], tokens[2]));
                edges.Add(new Edge(tokens[1], tokens[0], tokens[2]));
            }

            return edges;
        }

        private static List<Edge> GetRoutes(List<Edge> graph, int startPoint, int endPoint)
        {
            Dictionary<int, int> previouses = graph
                .Select(x => x.Start)
                .Union(graph.Select(x => x.End))
                .Distinct()
                .ToDictionary(x => x, x => x);

            var nodes = previouses.Keys.ToDictionary(x => x, x => graph.Where(e => e.Start == x).ToArray());

            Dictionary<int, double> travelCost = nodes.Keys.ToDictionary(x => x, x => double.MinValue);
            travelCost[startPoint] = 1;

            var queue = new List<int>();
            queue.Add(startPoint);
            while (queue.Any())
            {
                int nodeId = queue.OrderByDescending(x => travelCost[x]).First();
                queue.Remove(nodeId);

                foreach (Edge child in nodes[nodeId])
                {
                    int childId = child.End;

                    if (travelCost[childId] == double.MinValue) // Not visited enqueue!
                        queue.Add(childId);

                    if (travelCost[childId] < travelCost[nodeId] * child.Weight)
                    {
                        travelCost[childId] = travelCost[nodeId] * child.Weight;
                        previouses[childId] = nodeId;
                    }
                }
            }

            var result = new Stack<Edge>();
            while (endPoint != startPoint)
            {
                var start = previouses[endPoint];
                result.Push(graph.FirstOrDefault(x => x.End == endPoint && x.Start == start));
                endPoint = start;
            }

            return result.ToList();
        }
        private static void PrintResults(List<Edge> path)
        {
            double safety = 100;
            path.ForEach(x => safety *= x.Weight);
            Console.WriteLine($"Most reliable path reliability: {safety:F2}%");
            Console.WriteLine(string.Join(" -> ", path.Select(x => x.Start)) + " -> " + path.Last().End);
        }
        private class Edge
        {
            public Edge(int start, int end, double weight)
            {
                Start = start;
                End = end;
                Weight = weight / 100;
            }
            public int Start { get; }
            public int End { get; }
            public double Weight { get; }
        }
    }
}
