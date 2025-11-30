using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeStatsParallel.src.Stats
{
    public static class StatsAggregator
    {
        public static StatisticsResult BuildResult(
            ConcurrentDictionary<string, int> birthCountryStats,
            ConcurrentDictionary<string, int> currentCountryStats,
            ConcurrentDictionary<string, int> ageBucketStats,
            IEnumerable<int> ages)
        {
            var birthCountryDict = birthCountryStats
                .OrderBy(kv => kv.Key)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var currentCountryDict = currentCountryStats
                .OrderBy(kv => kv.Key)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var ageBucketDict = ageBucketStats
                .OrderBy(kv => kv.Key)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            int totalEmployees = birthCountryDict.Values.Sum();

            var topBirth = birthCountryStats
                .OrderByDescending(kv => kv.Value)
                .Take(3)
                .Select(kv => kv.Key)
                .ToList();

            var topCurrent = currentCountryStats
                .OrderByDescending(kv => kv.Value)
                .Take(3)
                .Select(kv => kv.Key)
                .ToList();

            double averageAge = 0.0;
            if (ages != null)
            {
                var list = ages.ToList();
                if (list.Count > 0)
                    averageAge = list.Average();
            }

            return new StatisticsResult
            {
                GeneratedAt = DateTime.UtcNow,
                EmployeesByCountryOfBirth = birthCountryDict,
                EmployeesByCurrentCountry = currentCountryDict,
                EmployeesByAgeBucket = ageBucketDict,
                TotalEmployees = totalEmployees,
                TopCountriesOfBirth = topBirth,
                TopCurrentCountries = topCurrent,
                AverageAge = averageAge
            };
        }
    }
}
