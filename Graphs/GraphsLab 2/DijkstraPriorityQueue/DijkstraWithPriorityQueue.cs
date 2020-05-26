namespace Dijkstra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class DijkstraWithPriorityQueue
    {
        public static List<int> DijkstraAlgorithm(Dictionary<Node, Dictionary<Node, int>> graph, Node sourceNode, Node destinationNode)
        {
            graph.Keys.ToList().ForEach(x =>
            {
                x.DistanceFromStart = double.PositiveInfinity;
            });

            var queue = new PriorityQueue<Node>();
            sourceNode.DistanceFromStart = 0;
            queue.Enqueue(sourceNode);

            var previous = new Dictionary<Node, Node>() { [sourceNode] = sourceNode };

            while (queue.Count > 0)
            {
                Node currentNode = queue.ExtractMin();
                Dictionary<Node, int> children = graph[currentNode];
                foreach (var kvp in children)
                {
                    if (kvp.Key.DistanceFromStart == double.PositiveInfinity)
                        queue.Enqueue(kvp.Key);

                    if (kvp.Key.DistanceFromStart < currentNode.DistanceFromStart + kvp.Value)
                        continue;

                    kvp.Key.DistanceFromStart = currentNode.DistanceFromStart + kvp.Value;
                    previous[kvp.Key] = currentNode;
                }
            }
            return GetPath(previous, sourceNode, destinationNode);
        }


        private static List<int> GetPath(Dictionary<Node, Node> map, Node sorce, Node destination)
        {
            if (destination.DistanceFromStart==double.PositiveInfinity)
                return null;
            List<int> result = new List<int>();
            while (destination != sorce)
            {
                result.Add(destination.Id);
                destination = map[destination];
            }

            result.Add(sorce.Id);
            result.Reverse();
            return result;

        }
    }
}
