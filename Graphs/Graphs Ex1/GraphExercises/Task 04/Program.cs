using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_04
{
    class Program
    {
        static void Main(string[] args)
        {
            int employeesCount = int.Parse(Console.ReadLine());
            var salaries = new int[employeesCount];
            var graph = new List<int>[employeesCount];

            for (int i = 0; i < employeesCount; i++)
            {
                graph[i] = new List<int>();
                var input = Console.ReadLine().Select(x => x == 'Y').ToArray();
                for (int j = 0; j < input.Length; j++)
                {
                    if (input[j])
                        graph[i].Add(j);
                }
            }

            var salaryZeroIndex = Array.IndexOf(salaries, 0);
            while (salaryZeroIndex != -1)
            {
                EvaluateSalaries(salaryZeroIndex, graph, salaries);
                salaryZeroIndex = Array.IndexOf(salaries, 0);
            }
            Console.WriteLine(salaries.Sum());
        }

        private static void EvaluateSalaries(int nodeIndex, List<int>[] graph, int[] salaries)
        {
            if (salaries[nodeIndex] != 0)
                return;

            if (graph[nodeIndex].Count() == 0)
            {
                salaries[nodeIndex] = 1;
                return;
            }

            foreach (int slave in graph[nodeIndex])
            {
                EvaluateSalaries(slave, graph, salaries);
            }

            salaries[nodeIndex] = graph[nodeIndex].Sum(x => salaries[x]);
        }
    }
}
