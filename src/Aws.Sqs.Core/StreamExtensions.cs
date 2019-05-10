using System.Buffers;
using System.IO;
using System.Threading.Tasks;

namespace HighPerfCloud.Aws.Sqs.Core
{
    internal static class StreamExtensions
    {
        internal static ValueTask<IMemoryOwner<byte>> RentAndPopulateAsync(this Stream stream,
            int contentLength)
        {
            return SqsReceiveResponseMemoryPool.RentAndPopulateFromStreamAsync(stream, contentLength);
        }
    }
}