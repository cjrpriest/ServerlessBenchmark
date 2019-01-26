using System;

namespace ServerlessBenchmark.Storage.Test.Rds
{
    public class S3Arguments
    {
        public int NumberOfDocsToRetrieve { get; set; }
        public string BucketName { get; set; }
        public string KeyNamesSourceFile { get; set; }
        public bool UseHttp { get; set; }

        public S3Arguments(){ }
        
        public S3Arguments(string[] args)
        {
            NumberOfDocsToRetrieve = Int32.Parse(args[1]);
            BucketName = args[2];
            KeyNamesSourceFile = args[3];
            UseHttp = bool.Parse(args[4]);
        }
    }
}