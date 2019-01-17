using System;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = new Arguments(args);
            var taskReport = new TaskOrchestrator().StartTask(
                ComputationallyIntensiveTask,
                TimeSpan.FromSeconds(arguments.LengthOfTestInSeconds),
                arguments.NoOfThreads);
            Console.WriteLine($"{taskReport.TotalIterationsCompleted} iterations completed");
        }

        private static void ComputationallyIntensiveTask()
        {
            PrimeNumberGenerator.CalculatePrimeNumbers(10000);
            FactorialCalculator.CalculateFactorial(5000);
        }
    }
}