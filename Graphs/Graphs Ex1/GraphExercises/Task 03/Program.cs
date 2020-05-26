using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_03
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Dictionary<string, List<string>>();
            var nodes = new HashSet<string>();
            var input = string.Empty;

            while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
            {
                var edge = input.Split(new[] { "–","-" },StringSplitOptions.RemoveEmptyEntries);
                string start = edge[0];
                string end = edge[1];

                nodes.Add(start);
                nodes.Add(end);
                if (!graph.ContainsKey(start))
                    graph[start] = new List<string>();

                if (!graph.ContainsKey(end))
                    graph[end] = new List<string>();

                graph[start].Add(end);
                graph[end].Add(start);//without this line it works for directed graphs
            }

            bool result = IsAcyclic(graph, nodes);
            Console.WriteLine($"Acyclic: {(IsAcyclic(graph, nodes) ? "Yes" : "No")}");

        }

        private static bool IsAcyclic(Dictionary<string, List<string>> graph, HashSet<string> nodes)
        {
            var graphTemp = graph.ToDictionary(x => x.Key, x => x.Value.ToList());
            var freeNodeQueue = new Queue<string>(
                nodes.Where(x => graphTemp.Values.SelectMany(t => t).Count(t => t == x) ==1));
            var resutSequence = new Queue<string>();

            while (freeNodeQueue.Any())
            {
                var currentId = freeNodeQueue.Dequeue();
                var children = graph[currentId];
                graphTemp.Remove(currentId);
                resutSequence.Enqueue(currentId);
                foreach (string child in children)
                {
                    if (graphTemp.Values.SelectMany(x => x).Count(x=>x==child)==1)
                        freeNodeQueue.Enqueue(child);
                }
            }

            return graphTemp.Count()==0;
        }
    }
}
