using System;
using System.Net;
using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ServerlessBenchmark.Storage.DataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            var noOfDocsToCreate = Int32.Parse(args[0]);
            Console.WriteLine($"Start time: {DateTime.Now}");

            switch (args[1])
            {
                case "S3":
                    LoadDataIntoS3(noOfDocsToCreate);
                    break;
                case "RDS":
                    LoadDataIntoRds(noOfDocsToCreate);
                    break;
            }

            Console.WriteLine($"End time: {DateTime.Now}");
        }

        private static void LoadDataIntoRds(int noOfDocsToCreate)
        {
            var connString = "Server=storage-serverlessbenchmark.cvswfjkqyiao.eu-west-1.rds.amazonaws.com;User ID=chris;Password=cartman4;Database=serverlessbenchmark";

            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();

                for (int i = 0; i < noOfDocsToCreate; i++)
                {
                    var doc = GenerateDocument(4096);

                    // Insert some data
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO documents (contents) VALUES (@c)";
                        cmd.Parameters.AddWithValue("c", doc.Contents);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void LoadDataIntoS3(int noOfDocsToCreate)
        {
            var s3Client = new AmazonS3Client(RegionEndpoint.EUWest1);
            for (int i = 0; i < noOfDocsToCreate; i++)
            {
                var doc = GenerateDocument(4096);
                var task = s3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = "storage-servelessbenchmark.cjrp.me",
                    Key = $"objects/{doc.KeyName}",
                    ContentBody = doc.Contents,
                });
                var putObjectResponse = task.Result;
                if (putObjectResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(putObjectResponse));
                    break;
                }
            }
        }

        private static Document GenerateDocument(int size)
        {
            var documentContents = new StringBuilder();
            while (documentContents.Length < size)
            {
                documentContents.Append(LoremIpsum(5, 20));
            }

            var truncatedDocContents = documentContents.ToString().Substring(0, size);
            return new Document
            {
                Contents = truncatedDocContents,
                KeyName = Guid.NewGuid().ToString()
            };
        }
        
        private static readonly string[] words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
            "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
            "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};
        private static readonly Random rand = new Random();

        static string LoremIpsum(int minWords, int maxWords)
        {
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for(int w = 0; w < numWords; w++) {
                if (w > 0) { result.Append(" "); }
                result.Append(words[rand.Next(words.Length)]);
            }
            result.Append(". ");

            return result.ToString();
        }
    }
}