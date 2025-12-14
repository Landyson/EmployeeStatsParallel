using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using EmployeeStatsParallel.src.Model;
using EmployeeStatsParallel.src.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeStatsParallel.Tests
{
    [TestClass]
    public class StatisticsCalculatorTests
    {
        [TestMethod]
        public void UpdateStatisticsForEmployee_UpdatesAllCollections()
        {
            var birthCountryStats = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var currentCountryStats = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var ageBucketStats = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var ages = new ConcurrentBag<int>();
            var ageBuckets = new List<int> { 25, 35, 45, 60 };

            var employee = new Employee
            {
                Id = "1",
                FirstName = "Test",
                LastName = "User",
                CountryOfBirth = "Czech Republic",
                CurrentCountry = "Germany",
                DateOfBirth = DateTime.Today.AddYears(-30) 
            };

            StatisticsCalculator.UpdateStatisticsForEmployee(
                employee,
                birthCountryStats,
                currentCountryStats,
                ageBucketStats,
                ageBuckets,
                ages);

            Assert.IsTrue(birthCountryStats.TryGetValue("Czech Republic", out int birthCount));
            Assert.AreEqual(1, birthCount);

            Assert.IsTrue(currentCountryStats.TryGetValue("Germany", out int currentCount));
            Assert.AreEqual(1, currentCount);

            Assert.IsTrue(ageBucketStats.ContainsKey("26-35"));
            Assert.IsFalse(ages.IsEmpty);
        }
    }
}
