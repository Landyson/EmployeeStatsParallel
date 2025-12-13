using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeStatsParallel.src.Config;
using EmployeeStatsParallel.src.Model;
using EmployeeStatsParallel.src.Pipeline;
using EmployeeStatsParallel.src.Stats;

namespace EmployeeStatsParallel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Employee Stats Parallel ===");

            string basePath = AppContext.BaseDirectory;

            string configPath = Path.Combine(basePath, "config", "config.json");

            if (!File.Exists(configPath))
            {
                Console.WriteLine($"Config file '{configPath}' not found.");
                return;
            }

            AppConfig config;
            try
            {
                config = ConfigLoader.Load(configPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load config: {ex.Message}");
                return;
            }

            
            string inputFile = config.InputFile;

            // Když je cesta relativní, přepočítám ji ke složce exe
            if (!string.IsNullOrWhiteSpace(inputFile) && !Path.IsPathRooted(inputFile))
            {
                inputFile = Path.Combine(basePath, inputFile);
            }

            // Když je v příkazové řádce --input nebo -i, použiju tuhle cestu
            if (args.Length >= 2 && (args[0] == "--input" || args[0] == "-i"))
            {
                string cliInput = args[1];

                // Zase převedu na absolutní cestu, pokud je relativní
                if (!Path.IsPathRooted(cliInput))
                {
                    inputFile = Path.Combine(basePath, cliInput);
                }
                else
                {
                    inputFile = cliInput;
                }
            }


            string outputFile = config.OutputFile;

            // Když není nic, dám defaultní výstup/output/stats.json
            if (string.IsNullOrWhiteSpace(outputFile))
            {
                outputFile = Path.Combine(basePath, "output", "stats.json");
            }
            // Když je cesta relativní, spojím ji se složkou exe
            else if (!Path.IsPathRooted(outputFile))
            {
                outputFile = Path.Combine(basePath, outputFile);
            }

            
            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Input file '{inputFile}' not found.");
                return;
            }

            
            if (config.WorkerCount <= 0)
            {
                config.WorkerCount = Environment.ProcessorCount;
            }

            
            if (config.AgeBuckets == null || config.AgeBuckets.Count == 0)
            {
                config.AgeBuckets = new List<int> { 25, 35, 45, 60 };
            }

            
            config.AgeBuckets.Sort();

            
            Console.WriteLine($"Base path: {basePath}");
            Console.WriteLine($"Config:    {configPath}");
            Console.WriteLine($"Input:     {inputFile}");
            Console.WriteLine($"Output:    {outputFile}");
            Console.WriteLine($"Workers:   {config.WorkerCount}");
            Console.WriteLine();

           

            var queue = new BlockingCollection<Employee>(boundedCapacity: 1000);

            var birthCountryStats = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            var currentCountryStats = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            var ageBucketStats = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
           
            var ages = new ConcurrentBag<int>();

            

            // Spustím více worker vláken, která čtou z fronty a počítají statistiky
            var workerTasks = EmployeeWorker.StartWorkers(
                config.WorkerCount,
                queue,
                birthCountryStats,
                currentCountryStats,
                ageBucketStats,
                config.AgeBuckets,
                ages);

            
            
            var producerTask = Task.Run(() => EmployeeProducer.ProduceEmployees(inputFile, queue));

            
            Task.WaitAll(workerTasks.Concat(new[] { producerTask }).ToArray());


            // Ze slovníků a věků vytvořím jeden objekt s výsledky
            var result = StatsAggregator.BuildResult(
                birthCountryStats,
                currentCountryStats,
                ageBucketStats,
                ages);


            if (!string.IsNullOrWhiteSpace(outputFile))
            {
                // Vytvořím složku output, když neexistuje
                var outputDir = Path.GetDirectoryName(outputFile);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true 
                };

                
                string json = JsonSerializer.Serialize(result, jsonOptions);

                
                File.WriteAllText(outputFile, json);
            }


            Console.WriteLine("=== Summary ===");
            Console.WriteLine($"Total employees: {result.TotalEmployees}");
            Console.WriteLine();

            if (config.ShowBirthCountryStats)
            {
                Console.WriteLine("Employees by country of birth:");
                foreach (var kv in result.EmployeesByCountryOfBirth.OrderByDescending(kv => kv.Value))
                    Console.WriteLine($"  {kv.Key}: {kv.Value}");
                Console.WriteLine();
            }

            if (config.ShowCurrentCountryStats)
            {
                Console.WriteLine("Employees by current country:");
                foreach (var kv in result.EmployeesByCurrentCountry.OrderByDescending(kv => kv.Value))
                    Console.WriteLine($"  {kv.Key}: {kv.Value}");
                Console.WriteLine();
            }

            if (config.ShowAgeBucketStats)
            {
                Console.WriteLine("Employees by age bucket:");
                foreach (var kv in result.EmployeesByAgeBucket.OrderBy(kv => kv.Key))
                    Console.WriteLine($"  {kv.Key}: {kv.Value}");
                Console.WriteLine();
            }

            if (config.ShowTopCountriesOfBirth)
            {
                Console.WriteLine("Top 3 countries of birth:");
                if (result.TopCountriesOfBirth.Count == 0)
                {
                    Console.WriteLine("  (no data)");
                }
                else
                {
                    foreach (var country in result.TopCountriesOfBirth)
                        Console.WriteLine($"  {country}");
                }
                Console.WriteLine();
            }

            if (config.ShowTopCurrentCountries)
            {
                Console.WriteLine("Top 3 current countries (where employees live):");
                if (result.TopCurrentCountries.Count == 0)
                {
                    Console.WriteLine("  (no data)");
                }
                else
                {
                    foreach (var country in result.TopCurrentCountries)
                        Console.WriteLine($"  {country}");
                }
                Console.WriteLine();
            }

            if (config.ShowAverageAge)
            {
                Console.WriteLine($"Average age: {result.AverageAge:F2}");
                Console.WriteLine();
            }

            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
