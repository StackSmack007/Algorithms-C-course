namespace Kurskal
{
    using System.Collections.Generic;
    using System.Linq;

    public class KruskalAlgorithm
    {
        public static List<Edge> Kruskal(int numberOfVertices, List<Edge> edges)
        {
            var nodesRoots = edges
                .Select(x => x.StartNode)
                .Union(edges.Select(x => x.EndNode))
                .Distinct()
                .ToDictionary(key => key, value => value);

            var minimumEdgesRequired = new List<Edge>(nodesRoots.Count());

            foreach (var edge in edges.OrderBy(x => x.Weight).ToList())
            {
                int startNodeRoot = FindRoot(edge.StartNode, nodesRoots);
                int endNodeRoot = FindRoot(edge.EndNode, nodesRoots);

                if (startNodeRoot == endNodeRoot)
                    continue;

                minimumEdgesRequired.Add(edge);
                nodesRoots[endNodeRoot] = edge.StartNode;
            }

            return minimumEdgesRequired;
        }

        public static int FindRoot(int node, Dictionary<int, int> parent)
        {
            while (parent[node] != node)
                node = parent[node];
            return node;
        }
    }
}