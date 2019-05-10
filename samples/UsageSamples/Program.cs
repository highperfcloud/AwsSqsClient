using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HighPerfCloud.Aws.Sqs.Core;

namespace UsageSamples
{
    internal class Program
    {
        private const string ResponseStart = @"<ReceiveMessageResponse><ReceiveMessageResult>";
        private const string ResponseEnd = @"</ReceiveMessageResult><ResponseMetadata><RequestId>b6633655-283d-45b4-aee4-4e84e0ae6afa</RequestId></ResponseMetadata></ReceiveMessageResponse>";
        private const string Message = @"<Message><MessageId>5fea7756-0ea4-451a-a703-a558b933e274</MessageId><ReceiptHandle>MbZj6wDWli=</ReceiptHandle><MD5OfBody>fafb00f5732ab283681e124bf8747ed1</MD5OfBody><Body>This is a test message</Body><Attribute><Name>SentTimestamp</Name><Value>1238099229000</Value></Attribute></Message>";

        private static async Task Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                await RentAndPopulateFromStreamAsync();
            }

            await RentAndPopulateFromStreamAsync();
         
            Console.WriteLine("Done 1");

            byte[] bytes = GetResponseBytes();

            await using var ms = new MemoryStream(bytes);

            using var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(ms)
            };

            using var reader = new LightweightMessageReader(response);

            await foreach (var message in reader)
            {
                Console.WriteLine(message.MessageId);
            }

            Console.WriteLine("Done 2");

            await using var ms2 = new MemoryStream(bytes);

            using var response2 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(ms2)
            };

            await using var contentStream = await response2.Content.ReadAsStreamAsync();

            if (response2.Content.Headers.ContentLength != null)
            {
                using var reader2 = new LightweightMessageReader(contentStream,
                    (int)response2.Content.Headers.ContentLength.Value);

                await foreach (var message in reader2)
                {
                    Console.WriteLine(message.MessageId);
                }
            }

            Console.WriteLine("Done 3");
        }

        private static async Task RentAndPopulateFromStreamAsync()
        {
            byte[] bytes = GetResponseBytes();

            await using var ms = new MemoryStream(bytes);

            using (var responseMemory = await SqsReceiveResponseMemoryPool.RentAndPopulateFromStreamAsync(ms, bytes.Length))
            {
                var memory = responseMemory.Memory; // We now have the bytes for the response that we can parse.
            }
        }

        private static byte[] GetResponseBytes()
        {
            var sb = new StringBuilder();
            sb.Append(ResponseStart);
            for (var i = 0; i < 3; i++)
            {
                sb.Append(Message);
            }
            sb.Append(ResponseEnd);

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return bytes;
        }
    }
}
