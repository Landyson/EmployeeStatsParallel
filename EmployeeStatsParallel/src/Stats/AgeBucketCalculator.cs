using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeStatsParallel.src.Stats
{
    public static class AgeBucketCalculator
    {
        public static string GetBucketLabel(DateTime dateOfBirth, IReadOnlyList<int> thresholds)
        {
            int age = CalculateAge(dateOfBirth);

            if (thresholds == null || thresholds.Count == 0)
                return age.ToString();

            int previousLower = 0;

            foreach (var limit in thresholds)
            {
                if (age <= limit)
                    return $"{previousLower}-{limit}";

                previousLower = limit + 1;
            }

            return $"{previousLower}+";
        }

        public static int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            int age = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
