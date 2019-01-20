using System;

namespace Runner
{
    public class Arguments
    {
        public int LengthOfTestInSeconds { get; set; }
        public int NoOfThreads { get; set; }

        public Arguments() { }
        
        public Arguments(string[] args)
        {
            LengthOfTestInSeconds = Int32.Parse(args[0]);
            NoOfThreads = Int32.Parse(args[1]);
        }
    }
}