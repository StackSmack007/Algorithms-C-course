using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab01
{
    class Program
    {
        static void Main(string[] args)
        {
            int nodeCount = int.Parse(Console.ReadLine());
            var graph = new List<int>[nodeCount];

            for (int i = 0; i < nodeCount; i++)
            {
                graph[i] = Console.ReadLine().Split(' ',StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            }


            bool[] visited = new bool[nodeCount];
            for (int i = 0; i < graph.Length; i++)
            {

                if (visited[i])
                    continue;

                List<int> nodes = new List<int>();
                DFS(i, graph, visited, nodes);
                Console.WriteLine($"Connected component: {string.Join(" ", nodes)}");

            }

        }

        private static void DFS(int node, List<int>[] graph, bool[] visited, List<int> nodes)
        {
            visited[node] = true;
            foreach (var child in graph[node])
            {
                if (visited[child])
                    continue;
                DFS(child, graph, visited, nodes);
            }

            nodes.Add(node);
        }
    }
}
