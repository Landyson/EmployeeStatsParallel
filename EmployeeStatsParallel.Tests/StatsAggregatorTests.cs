using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using EmployeeStatsParallel.src.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeStatsParallel.Tests
{
    [TestClass]
    public class StatsAggregatorTests
    {
        [TestMethod]
        public void BuildResult_ComputesTotalEmployees_Top3_AndAverageAge()
        {
            var birthCountryStats = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            birthCountryStats["CZ"] = 10;
            birthCountryStats["DE"] = 5;
            birthCountryStats["US"] = 2;

            var currentCountryStats = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            currentCountryStats["CZ"] = 8;
            currentCountryStats["DE"] = 6;
            currentCountryStats["UK"] = 3;

            var ageBucketStats = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            ageBucketStats["0-25"] = 4;
            ageBucketStats["26-35"] = 6;

            var ages = new List<int> { 20, 30, 40 };

            var result = StatsAggregator.BuildResult(
                birthCountryStats,
                currentCountryStats,
                ageBucketStats,
                ages);

            Assert.AreEqual(17, result.TotalEmployees); 

            Assert.AreEqual(30.0, result.AverageAge, 0.001);

            CollectionAssert.Contains(result.TopCountriesOfBirth, "CZ");
        }
    }
}
