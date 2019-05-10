using BenchmarkDotNet.Attributes;
using HighPerfCloud.Aws.Sqs.Core;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Aws.Sqs.Core.Benchmarks
{
    [MemoryDiagnoser]
    public class RentAndPopulateFromStreamAsyncBenchmarks
    {
        private Stream _stream;
        private int _contentLength;

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

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            _stream = new MemoryStream(bytes);
            _contentLength = bytes.Length;
        }

        [Params(0, 5, 10)]
        public int Count;

        //[Benchmark]
        //public async ValueTask AwaitRentAndPopulateFromStreamAsync()
        //{
        //    _stream.Position = 0;

        //    using (var responseMemory = await SqsReceiveResponseMemoryPool.RentAndPopulateFromStreamAsync(_stream, _contentLength))
        //    {
        //        var memory = responseMemory.Memory;
        //    }
        //}

        [Benchmark]
        public void AwaitRentAndPopulateFromStream()
        {
            _stream.Position = 0;

            using (var responseMemory = SqsReceiveResponseMemoryPool.RentAndPopulateFromStream(_stream, _contentLength))
            {
                var memory = responseMemory.Memory;
            }
        }

        [GlobalCleanup]
        public void Cleanup() => _stream.Dispose();
    }
}
