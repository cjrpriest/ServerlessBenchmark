using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Runner
{
    public class TaskOrchestrator
    {
        public TaskReport StartTask(Action taskToRepeat, TimeSpan forHowLong, int numberOfThreads)
        {
            Console.WriteLine($"Starting task for {forHowLong} on {numberOfThreads} threads...");
            
            var taskThreadReporters = new List<Func<TaskThreadReport>>();
            
            for (int i = 0; i < numberOfThreads; i++)
            {
                var threadReporter = StartRepeatingTask(taskToRepeat, forHowLong);
                taskThreadReporters.Add(threadReporter);
            }

            WaitUntilAllTaskThreadsComplete(taskThreadReporters);

            var totalIterations = taskThreadReporters.Sum(x => x().IterationsCompleted);

            return new TaskReport
            {
                TotalIterationsCompleted = totalIterations
            };
        }

        private static void WaitUntilAllTaskThreadsComplete(IReadOnlyCollection<Func<TaskThreadReport>> taskThreadReporters)
        {
            while (taskThreadReporters.Any(x => x().ThreadCompleted == false))
            {
                Thread.Sleep(1);
            }
        }

        private Func<TaskThreadReport> StartRepeatingTask(Action taskToRepeat, TimeSpan forHowLong)
        {
            var threadCompleted = false;
            var iterationsComplete = 0;

            var t = new Thread(() =>
            {
                var startTime = DateTimeOffset.Now;
                var endTime = startTime + forHowLong;
                while (DateTimeOffset.Now < endTime)
                {
                    taskToRepeat();
                    iterationsComplete++;
                }
                threadCompleted = true;
            });
            t.Start();

            return () => new TaskThreadReport
            {
                ThreadCompleted = threadCompleted,
                IterationsCompleted = iterationsComplete
            };
        }
    }
}