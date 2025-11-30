using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeStatsParallel.src.Config
{
    public class AppConfig
    {
        public string InputFile { get; set; } = "data/employees.json";
        public string OutputFile { get; set; } = "output/stats.json";
        public int WorkerCount { get; set; } = 4;
        public List<int> AgeBuckets { get; set; } = new List<int> { 25, 35, 45, 60 };
    }

    public static class ConfigLoader
    {
        public static AppConfig Load(string path)
        {
            string json = File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var config = JsonSerializer.Deserialize<AppConfig>(json, options);

            if (config == null)
                throw new InvalidOperationException("Config deserialized as null.");

            return config;
        }
    }
}
