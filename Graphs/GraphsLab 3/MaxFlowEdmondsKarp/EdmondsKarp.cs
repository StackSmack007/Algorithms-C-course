using System;
using System.Collections.Generic;
using System.Linq;

public class EdmondsKarp
{

    private static int[][] graph;
    private static int[][] capacitiesUsed;
    private static bool[] visited;
    private static int[] previous;

    public static int FindMaxFlow(int[][] targetGraph)
    {
        graph = targetGraph;
        int start = 0;
        int end = targetGraph.Length - 1;
        capacitiesUsed = targetGraph.Select(x => new int[x.Length]).ToArray();

        do
        {
            visited = new bool[targetGraph.Length];
            previous = new int[targetGraph.Length];
            DFS(start, end);
            UpdateUsedCapacities(end);

        } while (visited[end]);

        return capacitiesUsed.Sum(x => x[end]);
    }
    private static void DFS(int start, int end)
    {
        var queue = new Queue<int>();
        queue.Enqueue(start);
        visited[start] = true;
        while (queue.Any())
        {
            var node = queue.Dequeue();

            if (node == end)
                break;
            for (int i = 0; i < graph[node].Length; i++)
            {
                if (visited[i] || graph[node][i] == capacitiesUsed[node][i])//Pipe capacity depleted none or visited;
                    continue;
                visited[i] = true;
                queue.Enqueue(i);
                previous[i] = node;
            }
        }
    }
    private static void UpdateUsedCapacities(int end)
    {
        var path = new List<int>();
        while (previous[end] != end)
        {
            path.Add(end);
            end = previous[end];
        }
        path.Add(end);
        path.Reverse();

        int minCap = int.MaxValue;
        for (int i = 0; i < path.Count() - 1; i++)
        {
            int edgeStart = path[i];
            int edgeEnd = path[i + 1];
            minCap = Math.Min(minCap, graph[edgeStart][edgeEnd] - capacitiesUsed[edgeStart][edgeEnd]);
        }

        if (minCap == 0)
            return;

        for (int i = 0; i < path.Count() - 1; i++)
        {
            int edgeStart = path[i];
            int edgeEnd = path[i + 1];
            capacitiesUsed[edgeStart][edgeEnd] += minCap;
        }
    }
}
