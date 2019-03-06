using System.Collections.Generic;
using Amazon.Lambda.Core;
using ServerlessBenchmark.Storage.Test;
using ServerlessBenchmark.Storage.Test.Rds;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ServerlessBenchmark.Storage.LambdaClient
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<TestResult> FunctionHandler(Input input, ILambdaContext context)
        {
            var testRunner = new TestRunner();
            var testResult = testRunner.RunTest(input.Arguments);
            return testResult;
        }
    }
}
