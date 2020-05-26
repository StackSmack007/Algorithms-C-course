using System;
using System.Collections.Generic;
using System.Linq;

public class ArticulationPoints
{
    private static List<int>[] graph;
    private static int[] depths;
    private static int[] lowPoints;


    public static List<int> FindArticulationPoints(List<int>[] targetGraph)
    {
        graph = SanitizeGraph(targetGraph);
        depths = new int[targetGraph.Length];
        lowPoints = new int[targetGraph.Length];
        int start = 0;
        FillDeptsRecursive(start, start, 0);
        List<int> articulationPts = ExtractArticulationPts(start);
        return articulationPts;
    }

    private static List<int>[] SanitizeGraph(List<int>[] targetGraph)
    {
        var result = targetGraph.Select(x => new List<int>()).ToArray();
        for (int parent = 0; parent < targetGraph.Length; parent++)
        {
            foreach (int child in targetGraph[parent])
            {
                if (result[child].Contains(parent) || result[parent].Contains(child) || child == parent)
                    continue;
                result[parent].Add(child);
                result[child].Add(parent);
            }
        }

        return result;
    }
    private static void FillDeptsRecursive(int point, int root, int depth)
    {
        depths[point] = depth;
        foreach (int child in graph[point])
        {
            if (depths[child] > 0 || child == root)
                continue;
            FillDeptsRecursive(child, root, depth + 1);
        }

        if (point == root)
            return;

        lowPoints[point] = Math.Min(depth, graph[point].Select(x => depths[x]).Min());
    }
    private static List<int> ExtractArticulationPts(int start)
    {
        var result = new List<int>();
        for (int i = 0; i < graph.Length; i++)
        {
            bool isRoot = start == i;
            if (isRoot)
            {
                if (graph[i].Count() > 1)
                    result.Add(i);
                continue;
            }

            var depth = depths[i];
            var maxChildLowPoint = graph[i].Select(x => lowPoints[x]).Max();
            if (maxChildLowPoint >= depth)
                result.Add(i);
        }

        return result;
    }
}
