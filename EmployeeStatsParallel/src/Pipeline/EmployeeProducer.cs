using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeStatsParallel.src.Model;

namespace EmployeeStatsParallel.src.Pipeline
{
    public static class EmployeeProducer
    {
        public static void ProduceEmployees(
            string inputFile,
            BlockingCollection<Employee> queue)
        {
            Console.WriteLine($"Producer: loading employees from {inputFile} ...");

            string json = File.ReadAllText(inputFile);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var employees = JsonSerializer.Deserialize<List<Employee>>(json, options);

            if (employees == null || employees.Count == 0)
            {
                Console.WriteLine("Producer: no employees found in file.");
                queue.CompleteAdding();
                return;
            }

            int count = 0;
            foreach (var employee in employees)
            {
                queue.Add(employee);
                count++;
            }

            Console.WriteLine($"Producer: added {count} employees to queue.");

            // řeknu workerům, že už další položky nebudou
            queue.CompleteAdding();
        }
    }
}
