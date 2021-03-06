using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Runner;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaClient
{
    public class Function
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public TaskReport FunctionHandler(Arguments arguments, ILambdaContext context)
        {
            var taskReport = new TaskOrchestrator().StartTask(
                ComputationallyIntensive.Task,
                TimeSpan.FromSeconds(arguments.LengthOfTestInSeconds),
                arguments.NoOfThreads);
            return taskReport;
        }
    }
}