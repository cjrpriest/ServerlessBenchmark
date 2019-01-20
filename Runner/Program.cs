using System;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = new Arguments(args);
            var taskReport = new TaskOrchestrator().StartTask(
                ComputationallyIntensive.Task,
                TimeSpan.FromSeconds(arguments.LengthOfTestInSeconds),
                arguments.NoOfThreads);
            Console.WriteLine($"{taskReport.TotalIterationsCompleted} iterations completed");
        }
    }
}