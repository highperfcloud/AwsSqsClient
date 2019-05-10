using BenchmarkDotNet.Attributes;
using HighPerfCloud.Aws.Sqs.Core;
using System.Text;

namespace Aws.Sqs.Core.Benchmarks
{
    [MemoryDiagnoser]
    public class ReceiveMessageResponseReaderCountMessages
    {
        private byte[] _responseBytes;

        [GlobalSetup]
        public void Setup()
        {           
            var sb = new StringBuilder();
            sb.Append(Constants.ResponseStart);
            for (var i = 0; i <= Count; i++)
            {
                sb.Append(Constants.Message);
            }
            sb.Append(Constants.ResponseEnd);

            _responseBytes = Encoding.UTF8.GetBytes(sb.ToString());
        }

        [Params(0, 5, 10)]
        public int Count;

        [Benchmark]
        public int CountMessages()
        {
            var reader = new ReceiveMessageResponseReader();
            return reader.CountMessages(_responseBytes);
        }
    }
}
