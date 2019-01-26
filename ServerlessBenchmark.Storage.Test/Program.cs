using System;
using Newtonsoft.Json;

namespace ServerlessBenchmark.Storage.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var testResult = new TestRunner().RunTest(args);
            Console.WriteLine(JsonConvert.SerializeObject(testResult));
        }
    }
}