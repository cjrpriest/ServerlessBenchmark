using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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

        public IEnumerable<TestResult> RunTest()
        {
            var testResults = new List<TestResult>();
            for (int i = 0; i < _arguments.NoOfThreads; i++)
            {
                new Thread(() =>
                {
                    var testResult = RunSingleThreadTest();
                    testResults.Add(testResult);
                }).Start();
            }
            
            while (testResults.Count < _arguments.NoOfThreads) { Thread.Sleep(100); }

            return testResults;
        }

        private TestResult RunSingleThreadTest()
        {
            var responseTimes = new List<long>();
            var sw = new Stopwatch();
            sw.Start();
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
            sw.Stop();
            var average = responseTimes.Average();
            var tps = _arguments.NumberOfDocsToRetrieve / sw.Elapsed.TotalSeconds;
            return new TestResult
            {
                AverageResponseTime = average,
                Tps = tps
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