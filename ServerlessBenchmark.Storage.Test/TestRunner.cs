using System;
using System.Collections.Generic;
using ServerlessBenchmark.Storage.Test.Rds;
using ServerlessBenchmark.Storage.Test.RDS;

namespace ServerlessBenchmark.Storage.Test
{
    public class TestRunner
    {
        public IEnumerable<TestResult> RunTest(string[] arguments)
        {
            switch (arguments[0])
            {
                case "S3":
                    return new S3TestRunner(new S3Arguments(arguments)).RunTest();
                case "RDS":
                    return new RdsTestRunner(new RdsArguments(arguments)).RunTest();
            }
            throw new NotSupportedException();
        }
    }
}