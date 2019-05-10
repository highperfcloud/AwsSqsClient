using FluentAssertions;
using HighPerfCloud.Aws.Sqs.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Aws.Sqs.Core.Tests
{
    public class SqsReceiveResponseMemoryPoolTests
    {
        [Fact]
        public async Task RentAndPopulateFromStreamAsync_ShouldThrow_WhenStreamIsNotReadable()
        {
            var stream = new NonReadableStream();

            Func<Task> func = async () => await SqsReceiveResponseMemoryPool.RentAndPopulateFromStreamAsync(stream, 1);

            await func.Should().ThrowAsync<ArgumentException>(because: "the stream must be readable.");
        }

        [Fact]
        public async Task RentAndPopulateFromStreamAsync_ShouldThrow_WhenContentLengthIsZero()
        {
            var stream = new MemoryStream();

            Func<Task> func = async () => await SqsReceiveResponseMemoryPool.RentAndPopulateFromStreamAsync(stream, 0);

            await func.Should().ThrowAsync<ArgumentException>(because: "we should only both renting memory if there is content to store.");
        }

        [Fact]
        public async Task RentAndPopulateFromStreamAsync_ShouldReturnMemoryOwnerWithExpectedLength()
        {
            var stream = new MemoryStream(new byte[10]);

            var result = await SqsReceiveResponseMemoryPool.RentAndPopulateFromStreamAsync(stream, 5);

            result.Memory.Length.Should().Be(5);
        }

        [Fact]
        public void RentAndPopulateFromStream_ShouldThrow_WhenStreamIsNotReadable()
        {
            var stream = new NonReadableStream();

            Action act = () => SqsReceiveResponseMemoryPool.RentAndPopulateFromStream(stream, 1);

            act.Should().Throw<ArgumentException>(because: "the stream must be readable");
        }

        [Fact]
        public void RentAndPopulateFromStream_ShouldThrow_WhenContentLengthIsZero()
        {
            var stream = new MemoryStream();

            Action act = () => SqsReceiveResponseMemoryPool.RentAndPopulateFromStream(stream, 0);

            act.Should().Throw<ArgumentException>(because: "we should only both renting memory if there is content to store.");
        }

        [Fact]
        public void RentAndPopulateFromStream_ShouldReturnMemoryOwnerWithExpectedLength()
        {
            var stream = new MemoryStream();

            var result = SqsReceiveResponseMemoryPool.RentAndPopulateFromStream(stream, 5);

            result.Memory.Length.Should().Be(5);
        }

        private class NonReadableStream : Stream
        {
            public override bool CanRead => false;

            public override bool CanSeek => throw new NotImplementedException();

            public override bool CanWrite => throw new NotImplementedException();

            public override long Length => throw new NotImplementedException();

            public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }
        }
    }
}
