using System;
using System.Collections.Generic;
using System.Linq;

namespace Maximum_Tasks_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var workersCount = int.Parse(Console.ReadLine().Split(' ')[1]);
            var tasksCount = int.Parse(Console.ReadLine().Split(' ')[1]);
            var edges = ReadEdges(tasksCount, workersCount);

            var shedule = GetShedule(edges);
            PrintResult(shedule);
        }

        private static void PrintResult(List<JobEdge> shedule)
        {
            foreach (JobEdge job in shedule.OrderBy(x=>x.Worker))
            {
                Console.WriteLine($"{job.Worker}-{job.JobId}");
            }
        }

        private static List<JobEdge> GetShedule(List<JobEdge> edges)
        {
            edges = edges.ToList();
            var result = new List<JobEdge>();
            while (edges.Any())
            {
                int bestJob = edges.GroupBy(x => x.JobId)
                    .ToDictionary(x => x.Key, x => x.Count())
                    .OrderBy(x => x.Value)
                    .First()
                    .Key;

                var workerJob = edges
                    .Where(x => x.JobId == bestJob)
                    .OrderBy(x => edges.Count(e => e.Worker == x.Worker))
                    .First();
                result.Add(workerJob);
                edges = edges
                    .Where(x => x.Worker != workerJob.Worker && x.JobId != workerJob.JobId)
                    .ToList();
            }

            return result;
        }

        private static List<JobEdge> ReadEdges(int tasksCount, int workersCount)
        {
            var result = new List<JobEdge>();
            for (char worker = 'A'; worker < 'A' + workersCount; worker++)
            {
                var input = Console.ReadLine();
                for (int j = 0; j < input.Length; j++)
                {
                    if (input[j] == 'Y')
                    {
                        result.Add(new JobEdge()
                        {
                            Worker = worker,
                            JobId = j + 1
                        });
                    }
                }
            }

            return result;
        }
    }

    internal class JobEdge
    {
        public char Worker { get; set; }
        public int JobId { get; set; }
    }
}
