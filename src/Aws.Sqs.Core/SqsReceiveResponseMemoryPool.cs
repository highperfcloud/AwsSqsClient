using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HighPerfCloud.Aws.Sqs.Core
{
    internal static class SqsReceiveResponseMemoryPool
    {
        internal static ValueTask<IMemoryOwner<byte>> RentAndPopulateFromStreamAsync(Stream stream, int contentLength)
        {
            ValidateParameters(stream, contentLength);

            var buffer = ArrayPool<byte>.Shared.Rent(contentLength);

            var readTask = stream.ReadAsync(buffer);

            return readTask.IsCompletedSuccessfully
                ? new ValueTask<IMemoryOwner<byte>>(new SqsResponseMemoryOwner(buffer, contentLength))
                : AwaitAndReturnAsync(readTask, buffer, contentLength);

            static async ValueTask<IMemoryOwner<byte>> AwaitAndReturnAsync(ValueTask<int> runningTask, byte[] localBuffer, int contentLength)
            {
                await runningTask.ConfigureAwait(false);

                return new SqsResponseMemoryOwner(localBuffer, contentLength);
            }
        }        

        internal static IMemoryOwner<byte>? RentAndPopulateFromStream(Stream stream, int contentLength)
        {
            ValidateParameters(stream, contentLength);

            var buffer = ArrayPool<byte>.Shared.Rent(contentLength);

            stream.Read(buffer);

            return new SqsResponseMemoryOwner(buffer, contentLength);
        }

        private static void ValidateParameters(Stream stream, int contentLength)
        {
            if (!stream.CanRead)
                throw new ArgumentException(message: "The stream must be readable", paramName: nameof(stream));

            if (contentLength <= 0)
                throw new ArgumentException(message: "Content length must be a non-negative value, greater than 1", paramName: nameof(contentLength));
        }

        private sealed class SqsResponseMemoryOwner : IMemoryOwner<byte>
        {
            private readonly int _length;
            private byte[]? _oversized;

            internal SqsResponseMemoryOwner(byte[] oversized, int length)
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
