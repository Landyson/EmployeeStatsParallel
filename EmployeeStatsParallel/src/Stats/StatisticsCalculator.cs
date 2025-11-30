using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeStatsParallel.src.Model;

namespace EmployeeStatsParallel.src.Stats
{
    public static class StatisticsCalculator
    {
        public static void UpdateStatisticsForEmployee(
            Employee employee,
            ConcurrentDictionary<string, int> birthCountryStats,
            ConcurrentDictionary<string, int> currentCountryStats,
            ConcurrentDictionary<string, int> ageBucketStats,
            IReadOnlyList<int> ageBuckets,
            ConcurrentBag<int> ages)
        {
            if (!string.IsNullOrWhiteSpace(employee.CountryOfBirth))
            {
                string key = employee.CountryOfBirth.Trim();
                birthCountryStats.AddOrUpdate(key, 1, (_, oldValue) => oldValue + 1);
            }

            if (!string.IsNullOrWhiteSpace(employee.CurrentCountry))
            {
                string key = employee.CurrentCountry.Trim();
                currentCountryStats.AddOrUpdate(key, 1, (_, oldValue) => oldValue + 1);
            }

            string ageBucket = AgeBucketCalculator.GetBucketLabel(employee.DateOfBirth, ageBuckets);
            ageBucketStats.AddOrUpdate(ageBucket, 1, (_, oldValue) => oldValue + 1);

            int age = AgeBucketCalculator.CalculateAge(employee.DateOfBirth);
            ages.Add(age);
        }
    }
}
