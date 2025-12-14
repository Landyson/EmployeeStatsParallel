using System;
using System.Collections.Generic;
using EmployeeStatsParallel.src.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeStatsParallel.Tests
{
    [TestClass]
    public class AgeBucketCalculatorTests
    {
        [TestMethod]
        public void GetBucketLabel_20YearsOld_FallsInto_0_25()
        {
            var thresholds = new List<int> { 25, 35, 45, 60 };
            var dob = DateTime.Today.AddYears(-20); 

            string bucket = AgeBucketCalculator.GetBucketLabel(dob, thresholds);

            Assert.AreEqual("0-25", bucket);
        }

        [TestMethod]
        public void GetBucketLabel_40YearsOld_FallsInto_36_45()
        {
            var thresholds = new List<int> { 25, 35, 45, 60 };
            var dob = DateTime.Today.AddYears(-40); 

            string bucket = AgeBucketCalculator.GetBucketLabel(dob, thresholds);

            Assert.AreEqual("36-45", bucket);
        }
    }
}
