using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HighPerfCloud.Aws.Sqs.Core
{
    internal static class SqsMessageMemoryPool
    {
        internal static ValueTask<IMemoryOwner<byte>> RentAndPopulateFromStreamAsync(Stream stream, int contentLength)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(contentLength);

            var readTask = stream.ReadAsync(buffer);

            if (readTask.IsCompletedSuccessfully)
                return new ValueTask<IMemoryOwner<byte>>(new SqsMessageMemoryOwner(buffer, contentLength));

            return AwaitAndReturnAsync(readTask, buffer);

            async ValueTask<IMemoryOwner<byte>> AwaitAndReturnAsync(ValueTask<int> runningTask, byte[] localBuffer)
            {
                await runningTask.ConfigureAwait(false);

                return new SqsMessageMemoryOwner(localBuffer, contentLength);
            }
        }

        private struct SqsMessageMemoryOwner : IMemoryOwner<byte>
        {
            private readonly int _length;
            private byte[]? _oversized;

            internal SqsMessageMemoryOwner(byte[] oversized, int length)
            {
                _length = length;
                _oversized = oversized;
            }

            public Memory<byte> Memory => new Memory<byte>(GetArray(), 0, _length);

            private byte[] GetArray() =>
                Interlocked.CompareExchange(ref _oversized, null, null) ?? throw new ObjectDisposedException(ToString());

            public void Dispose()
            {
                var arr = Interlocked.Exchange(ref _oversized, null);
                if (arr != null) ArrayPool<byte>.Shared.Return(arr);
            }
        }
    }
}
