using System;
using System.Collections.Generic;
using System.Linq;

public class TopologicalSorter
{
    private Dictionary<string, List<string>> graph;

    public TopologicalSorter(Dictionary<string, List<string>> graph)
    {
        this.graph = graph.ToDictionary(x => x.Key, x => x.Value);
    }

    private IEnumerable<string> vertixesFree =>
        graph.Where(x => !graph.Values.SelectMany(e => e).Contains(x.Key)).Select(x => x.Key);


    public ICollection<string> TopSort1()
    {
        var result = new List<string>();
        var freeNodeKey = vertixesFree.FirstOrDefault();
        if (freeNodeKey is null)
            throw new InvalidOperationException("No elements for sorting or 1 cycle graph");
//Eto nqkva promqna
        while (graph.Any())
        {
            var children = graph[freeNodeKey];
            result.Add(freeNodeKey);
            graph.Remove(freeNodeKey);
            freeNodeKey = vertixesFree.FirstOrDefault(x => children.Contains(x));

            if (freeNodeKey is null && graph.Any())
                throw new InvalidOperationException("Cycle in graph");
        }

        return result;

    }

    public ICollection<string> TopSort()
    {
        var sorted = new List<string>(graph.Count());

        var remaining = graph.Where(x => !sorted.Contains(x.Key));
        while (graph.Count() > sorted.Count)
        {
            string freeNode = remaining
                .Where(x => !remaining.SelectMany(r => r.Value).Contains(x.Key))
                .Select(x => x.Key)
                .FirstOrDefault();

            if (freeNode is null)
                throw new InvalidOperationException("Cycle in graph is present, sorting not possible");

            sorted.Add(freeNode);
        }

        return sorted;
    }


}
