using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EmployeeStatsParallel.src.Model
{
    public class Employee
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = "";

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = "";

        [JsonPropertyName("countryOfBirth")]
        public string CountryOfBirth { get; set; } = "";

        [JsonPropertyName("currentCountry")]
        public string CurrentCountry { get; set; } = "";

        [JsonPropertyName("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }
    }
}
