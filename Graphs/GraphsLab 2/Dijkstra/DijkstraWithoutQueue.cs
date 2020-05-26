namespace Dijkstra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class DijkstraWithoutQueue
    {
        private const int INFINITY = int.MaxValue;

        public static List<int> DijkstraAlgorithm(int[,] graph, int sourceNode, int destinationNode)
        {
            int[] totalDistances = (new int[graph.GetLength(0)]).Select(x => INFINITY).ToArray();
            int[] previouses = (new int[graph.GetLength(0)]).Select(x => -1).ToArray();

            totalDistances[sourceNode] = 0;
            previouses[sourceNode] = 0;
            var nodesToProcess = new HashSet<int>(new[] { sourceNode });

            var edges = GetAllEdges(graph);
            while (nodesToProcess.Any())
            {
                int nodeId = nodesToProcess.OrderBy(x => totalDistances[x]).First();
                nodesToProcess.Remove(nodeId);

                foreach (Edge edge in edges[nodeId])
                {
                    if (totalDistances[edge.TargetId] == INFINITY)
                        nodesToProcess.Add(edge.TargetId);

                    if (totalDistances[edge.TargetId] <= totalDistances[nodeId] + edge.Weight)
                        continue;

                    previouses[edge.TargetId] = nodeId;
                    totalDistances[edge.TargetId] = totalDistances[nodeId] + edge.Weight;
                }
            }

            return GetShortestPath(sourceNode, destinationNode, previouses);
        }

        private static Dictionary<int, List<Edge>> GetAllEdges(int[,] graph)
        {
            var result = new Dictionary<int, List<Edge>>(graph.GetLength(0));

            for (int row = 0; row < graph.GetLength(0); row++)
            {
                var edges = new List<Edge>();
                for (int col = 0; col < graph.GetLength(1); col++)
                {
                    int weight = graph[row, col];
                    if (weight == 0)
                        continue;
                    edges.Add(new Edge(col, weight));
                }

                result.Add(row, edges);
            }

            return result;
        }
        private static List<int> GetShortestPath(int sourceNode, int destinationNode, int[] previouses)
        {
            if (previouses[sourceNode] == -1 || previouses[destinationNode] == -1)
                return null;

            var result = new Stack<int>();
            result.Push(destinationNode);
            while (result.Peek() != sourceNode)
            {
                result.Push(previouses[result.Peek()]);
            }

            return result.ToList();
        }
    }

    internal class Edge
    {
        public Edge(int targetId, int weight)
        {
            TargetId = targetId;
            Weight = weight;
        }

        public int TargetId { get; set; }
        public int Weight { get; set; }
    }
}
