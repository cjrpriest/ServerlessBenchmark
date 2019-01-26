using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace ServerlessBenchmark.Storage.Test.Rds
{
    public class S3TestRunner
    {
        private readonly Random _random = new Random();
        private readonly AmazonS3Client _amazonS3Client;
        private readonly S3Arguments _s3Arguments;

        
        public S3TestRunner(S3Arguments s3Arguments)
        {
            _s3Arguments = s3Arguments;
            _amazonS3Client = new AmazonS3Client(new AmazonS3Config
            {
                ForcePathStyle = true,
                UseHttp = _s3Arguments.UseHttp,
                RegionEndpoint = RegionEndpoint.EUWest1
            });
        }
        
        public TestResult RunTest()
        {
            Console.WriteLine("start test");

            var responseTimes = new List<long>();
            var objectKeyNames = GetObjectKeyNames().ToArray();
            for (var i = 0; i < _s3Arguments.NumberOfDocsToRetrieve; i++)
            {
                var randomKey = GetRandomKey(objectKeyNames);
                var responseTime = GetObject(randomKey);
                responseTimes.Add(responseTime);
            }

            var average = responseTimes.Average();
            return new TestResult
            {
                AverageResponseTime = average
            };
        }
        
        private IReadOnlyCollection<string> GetObjectKeyNames()
        {
            var objectKeynames = new List<string>();
            using (var fileStream = File.Open(_s3Arguments.KeyNamesSourceFile, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fileStream))
            {
                var objectList = streamReader.ReadToEnd();
                var objectListLines = objectList.Split(Environment.NewLine);
                foreach (var objectListLine in objectListLines)
                {
                    var objectsInLine = objectListLine.Split("\t");
                    foreach (var objectInLine in objectsInLine)
                    {
                        if (!String.IsNullOrEmpty(objectInLine))
                            objectKeynames.Add(objectInLine);
                    }
                }
            }

            Console.WriteLine("finished loading object keys");
            return objectKeynames;
        }
        
        private string GetRandomKey(IReadOnlyList<string> keys)
        {
            var randomIndex = _random.Next(keys.Count - 1);
            return keys[randomIndex];
        }
        
        private long GetObject(string objectKey)
        {
            Console.WriteLine($"getting object {objectKey}");
            Console.WriteLine($"{_s3Arguments.BucketName}");
            var sw = new Stopwatch();
            sw.Start();
            var task =  _amazonS3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = _s3Arguments.BucketName,
                Key = objectKey,
            });
            Console.WriteLine("before task.Result");
            var getObjectResponse = task.Result;
            Console.WriteLine("after task.Result");
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}