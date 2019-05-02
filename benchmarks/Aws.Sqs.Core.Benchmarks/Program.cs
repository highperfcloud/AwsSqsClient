using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using HighPerfCloud.Aws.Sqs.Core;
using System.Text;

namespace Aws.Sqs.Core.Benchmarks
{
    class Program
    {
        static void Main(string[] args) => _ = BenchmarkRunner.Run<ReceiveMessageResponseReader_CountMessages>();
    }

    [MemoryDiagnoser]
    public class ReceiveMessageResponseReader_CountMessages
    {
        private byte[] _responseBytes;

        [GlobalSetup]
        public void Setup()
        {            
            var responseStart = @"<ReceiveMessageResponse><ReceiveMessageResult>";
            var responseEnd = @"</ReceiveMessageResult><ResponseMetadata><RequestId>b6633655-283d-45b4-aee4-4e84e0ae6afa</RequestId></ResponseMetadata></ReceiveMessageResponse>";
            var message = @"<Message><MessageId>5fea7756-0ea4-451a-a703-a558b933e274</MessageId><ReceiptHandle>MbZj6wDWli=</ReceiptHandle><MD5OfBody>fafb00f5732ab283681e124bf8747ed1</MD5OfBody><Body>This is a test message</Body><Attribute><Name>SentTimestamp</Name><Value>1238099229000</Value></Attribute></Message>";

            var sb = new StringBuilder();
            sb.Append(responseStart);
            for (int i = 0; i <= Count; i++)
            {
                sb.Append(message);
            }
            sb.Append(responseEnd);

            _responseBytes = Encoding.UTF8.GetBytes(sb.ToString());
        }

        [Params(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        public int Count;

        [Benchmark]
        public int CountMessages()
        {
            var reader = new ReceiveMessageResponseReader();
            return reader.CountMessages(_responseBytes);
        }
    }
}
