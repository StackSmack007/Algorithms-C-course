using System.Collections.Generic;
using System.Linq;

public class StronglyConnectedComponents
{
    private static List<List<int>> stronglyConnectedComponents;
    private static bool[] visited;

    public static List<List<int>> FindStronglyConnectedComponents(List<int>[] targetGraph)
    {

        stronglyConnectedComponents = new List<List<int>>();
        var nodesInComponents = new Stack<int>();
        visited = new bool[targetGraph.Length];
        for (int i = 0; i < targetGraph.Length; i++)
        {
            if (visited[i])
                continue;
            List<int> component = new List<int>();
            FillAccessibleNodes(component, targetGraph, i);
            component.ForEach(x => nodesInComponents.Push(x));
        }

        visited = new bool[targetGraph.Length];
        var reversedGraph = targetGraph.Select(x => new List<int>()).ToArray();
        for (int i = 0; i < targetGraph.Length; i++)
        {
            int end = i;
            foreach (int start in targetGraph[i])
            {
                reversedGraph[start].Add(i);
            }
        }

        foreach (var nodeId in nodesInComponents)
        {
            if (visited[nodeId])
                continue;

            List<int> component = new List<int>();
            FillAccessibleNodes(component, reversedGraph, nodeId);
            stronglyConnectedComponents.Add(component.OrderBy(x=>x).ToList());
        }

        return stronglyConnectedComponents.OrderBy(x=>x.Count()).ThenBy(x=>x[0]).ToList();
    }

    private static void FillAccessibleNodes(List<int> repository, List<int>[] graph, int nodeId)
    {
        visited[nodeId] = true;
        foreach (var childId in graph[nodeId])
        {
            if (visited[childId])
                continue;
            FillAccessibleNodes(repository, graph, childId);
        }

        repository.Add(nodeId);
    }
}