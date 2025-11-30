using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeStatsParallel.src.Model;
using EmployeeStatsParallel.src.Stats;

namespace EmployeeStatsParallel.src.Pipeline
{
    public static class EmployeeWorker
    {
        public static List<Task> StartWorkers(
            int workerCount,
            BlockingCollection<Employee> queue,
            ConcurrentDictionary<string, int> birthCountryStats,
            ConcurrentDictionary<string, int> currentCountryStats,
            ConcurrentDictionary<string, int> ageBucketStats,
            IReadOnlyList<int> ageBuckets,
            ConcurrentBag<int> ages)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < workerCount; i++)
            {
                int workerId = i + 1;

                var task = Task.Run(() =>
                {
                    Console.WriteLine($"Worker {workerId}: started.");

                    // GetConsumingEnumerable čte, dokud producent neukončí frontu
                    foreach (var employee in queue.GetConsumingEnumerable())
                    {
                        StatisticsCalculator.UpdateStatisticsForEmployee(
                            employee,
                            birthCountryStats,
                            currentCountryStats,
                            ageBucketStats,
                            ageBuckets,
                            ages);
                    }

                    Console.WriteLine($"Worker {workerId}: finished.");
                });

                tasks.Add(task);
            }

            return tasks;
        }
    }
}
