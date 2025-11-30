using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeStatsParallel.src.Stats
{
    public class StatisticsResult
    {
        public int TotalEmployees { get; set; }

        public Dictionary<string, int> EmployeesByCountryOfBirth { get; set; } =
            new Dictionary<string, int>();

        public Dictionary<string, int> EmployeesByCurrentCountry { get; set; } =
            new Dictionary<string, int>();

        public Dictionary<string, int> EmployeesByAgeBucket { get; set; } =
            new Dictionary<string, int>();

        public DateTime GeneratedAt { get; set; }

        public List<string> TopCountriesOfBirth { get; set; } = new List<string>();

        public List<string> TopCurrentCountries { get; set; } = new List<string>();

        public double AverageAge { get; set; }
    }
}
