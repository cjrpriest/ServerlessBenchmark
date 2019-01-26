using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MySql.Data.MySqlClient;

namespace ServerlessBenchmark.Storage.Test.RDS
{
    public class RdsTestRunner
    {
        private readonly Random _random = new Random();

        private readonly string _connectionString;
        
        private readonly RdsArguments _arguments;

        public RdsTestRunner(RdsArguments arguments)
        {
            _arguments = arguments;
            _connectionString = $"Server={_arguments.ServerName};User ID={_arguments.UserId};Password={_arguments.Password};Database={_arguments.Database}";
        }

        public TestResult RunTest()
        {
            var responseTimes = new List<long>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                for (var i = 0; i < _arguments.NumberOfDocsToRetrieve; i++)
                {
                    var randomKey = GetRandomId();
                    var responseTime = GetObject(randomKey, conn);
                    responseTimes.Add(responseTime);
                }
            }

            var average = responseTimes.Average();
            return new TestResult
            {
                AverageResponseTime = average
            };

        }

        private long GetObject(long randomId, MySqlConnection conn)
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var cmd = new MySqlCommand($"SELECT * FROM documents WHERE id = {randomId}", conn))
            using (var reader = cmd.ExecuteReader())
            {
                // do nothing
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private long GetRandomId()
        {
            return _random.Next(100000);
        }
    }
}