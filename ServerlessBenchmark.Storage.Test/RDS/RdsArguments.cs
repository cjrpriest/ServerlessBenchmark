using System;

namespace ServerlessBenchmark.Storage.Test.RDS
{
    public class RdsArguments
    {
        public int NumberOfDocsToRetrieve { get; set; }
        public string ServerName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public int NoOfThreads { get; set; }

        public RdsArguments() { }

        public RdsArguments(string[] args)
        {
            NumberOfDocsToRetrieve = Int32.Parse(args[1]);
            ServerName = args[2];
            UserId = args[3];
            Password = args[4];
            Database = args[5];
            NoOfThreads = Int32.Parse(args[6]);
        }
    }
}