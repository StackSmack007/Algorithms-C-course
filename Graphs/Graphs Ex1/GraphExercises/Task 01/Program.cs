using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_01
{
    class Program
    {
        static void Main()
        {
            var condition = ReadInput();
            Dictionary<int, int[]> graph = condition.Item1;
            var routesDemanded = condition.Item2;

            foreach (var item in routesDemanded)
            {
                int routeLength = FindShortestPathLength(graph, item.Key, item.Value);
                Console.WriteLine($"{{{item.Key}, {item.Value}}} -> {routeLength}");
            }
        }

        private static int[] steps;
        private static bool[] visited;

        private static (Dictionary<int, int[]>, List<KeyValuePair<int, int>>) ReadInput()
        {
            int nodeCount = int.Parse(Console.ReadLine());
            int pathsCount = int.Parse(Console.ReadLine());

            Dictionary<int, int[]> graph = new Dictionary<int, int[]>(nodeCount);
            List<KeyValuePair<int, int>> routesDemanded = new List<KeyValuePair<int, int>>(pathsCount);

            for (int i = 0; i < nodeCount; i++)
            {
                string[] tokens = Console.ReadLine().Split(':');
                var origin = int.Parse(tokens[0]);
                var edges = tokens[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                graph[origin] = edges;
            }

            for (int i = 0; i < pathsCount; i++)
            {
                string[] tokens = Console.ReadLine().Split('-');
                routesDemanded.Add(new KeyValuePair<int, int>(int.Parse(tokens[0]), int.Parse(tokens[1])));
            }
            return (graph, routesDemanded);
        }
        private static int FindShortestPathLength(Dictionary<int, int[]> graph, int sorce, int destination)
        {
            if (destination == sorce)
                return 0;
            if (!graph.Keys.Contains(sorce) || !graph.Keys.Contains(destination))
                return -1;

            steps = new int[graph.Keys.Max() + 1];
            for (int i = 0; i < steps.Length; i++)
                steps[i] = int.MaxValue;
            //  Array.Fill(steps, int.MaxValue);
            visited = new bool[graph.Keys.Max() + 1];
            steps[sorce] = 0;

            PathLengthDFS(graph, sorce, destination);

            return steps[destination] == int.MaxValue ? -1 : steps[destination];
        }

        private static void PathLengthDFS(Dictionary<int, int[]> graph, int sorce, int destination)
        {
            if (steps[sorce] >= steps[destination])
            {
                return;
            }
            visited[sorce] = true;
            foreach (var child in graph[sorce])
            {
                if (visited[child] && steps[child] < steps[sorce] + 1)
                {
                    continue;
                }

                steps[child] = Math.Min(steps[child], steps[sorce] + 1);
                if (sorce == destination)
                    return;
                PathLengthDFS(graph, child, destination);
            }
        }

    }
}
