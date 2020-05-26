using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_05
{
    public class Program
    {
        private class Edge
        {
            public Edge(string start, string end)
            {
                Start = start;
                End = end;
            }
            public string Start { get; }
            public string End { get; }
            public override string ToString() =>
                $"{Start} - {End}";
        }

        static void Main(string[] args)
        {
            string input = string.Empty;
            List<string> nodes = new List<string>();
            var edges = new List<Edge>();

            while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
            {
                string[] tokens = input
                    .Split(new[] { "->", " " }, StringSplitOptions.RemoveEmptyEntries);
                nodes.Add(tokens[0]);

                for (int i = 1; i < tokens.Length; i++)
                {
                    var start = tokens[0];
                    var end = tokens[i];
                    var reversedEdgeEnlisted = edges.FirstOrDefault(x => x.Start == end && x.End == start);
                    if (reversedEdgeEnlisted is null)
                    {
                        edges.Add(new Edge(start, end));
                        continue;
                    }
                    
                    if (end.CompareTo(start) == 1) // new edge is better than the old one!
                    {
                        edges.Remove(reversedEdgeEnlisted);
                        edges.Add(new Edge(start, end));
                    }
                }
            }

            edges = edges.OrderByDescending(x => x.Start).ThenByDescending(x => x.End).ToList();

            var expendableEdges = GetExpendableEdges(nodes, edges.ToList());
            Console.WriteLine($"Edges to remove: {expendableEdges.Count()}");
            Console.WriteLine(string.Join(Environment.NewLine, expendableEdges.Select(x => x.ToString())));
        }

        private static ICollection<Edge> GetExpendableEdges(List<string> nodes, List<Edge> edges)
        {
            Dictionary<string, string> parents = nodes.ToDictionary(x => x, x => x);
            var expendableEdges = new Stack<Edge>();
            for (int i = 0; i < edges.Count; i++)
            {
                Edge edge = edges[i];
                var startRoot = GetRoot(parents, edge.Start);
                var endRoot = GetRoot(parents, edge.End);
                if (startRoot == endRoot)
                {
                    expendableEdges.Push(edge);
                    continue;
                }

                parents[endRoot] = edge.Start;
            }

            return expendableEdges.ToArray();
        }

        private static string GetRoot(Dictionary<string, string> parents, string nodeId)
        {
            while (parents[nodeId] != nodeId)
                nodeId = parents[nodeId];

            return nodeId;
        }
    }
}