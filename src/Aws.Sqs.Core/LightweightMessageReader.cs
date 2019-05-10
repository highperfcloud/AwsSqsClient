using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using HighPerfCloud.Aws.Sqs.Core.Primitives;

namespace HighPerfCloud.Aws.Sqs.Core
{
    internal sealed class LightweightMessageReader : IAsyncEnumerable<LightweightMessage>, IDisposable
    {
        private readonly HttpContent? _httpContent;
        private Stream? _stream;
        private IMemoryOwner<byte>? _responseBytes;
        private int _position;
        private bool _disposed;
        private readonly int _contentLength;

        public LightweightMessageReader(HttpResponseMessage response) : this(response?.Content)
        {
        }

        public LightweightMessageReader(HttpContent? httpContent)
        {
            _httpContent = httpContent ?? throw new ArgumentNullException(nameof(httpContent));

            _contentLength =
                _httpContent.Headers.ContentLength.HasValue &&
                _httpContent.Headers.ContentLength.Value > 0
                    ? (int)_httpContent.Headers.ContentLength.Value
                    : 0;
        }

        public LightweightMessageReader(Stream stream, int contentLength)
        {
            if (contentLength < 0)
                throw new ArgumentException(message: "The content length cannot be a negative value", paramName: nameof(contentLength));

            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _contentLength = contentLength;
        }

        public async IAsyncEnumerator<LightweightMessage> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            if (_stream == null)
            {
                _stream = await _httpContent!.ReadAsStreamAsync().ConfigureAwait(false);
            }

            if (_responseBytes == null)
            {
                _responseBytes = await _stream.RentAndPopulateAsync(_contentLength).ConfigureAwait(false);
            }

            while (TryGetNextMessageId(out var message))
            {
                yield return message!; // can't be null if TryGetNextMessageId is true
            }

            bool TryGetNextMessageId(out LightweightMessage? lightweightMessage)
            {
                var nextSlice = _responseBytes!.Memory.Span.Slice(_position);

                var reader = new ReceiveMessageResponseReader();

                if (reader.TryGetNextMessageBytes(nextSlice, out var message, out var position))
                {
                    _position += position;

                    lightweightMessage = GetLightweightMessageFromBytes(message);

                    return true;
                }

                lightweightMessage = null;
                return false;
            }
        }

        private static LightweightMessage GetLightweightMessageFromBytes(ReadOnlySpan<byte> messageBytes)
        {
            var messageParser = new MessageBytesParser(messageBytes);

            var messageIdBytes = messageParser.GetMessageIdBytesSpan();

            var messageId = new MessageId(messageIdBytes);

            return new LightweightMessage(messageId);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            var responseBytes = Interlocked.Exchange(ref _responseBytes, null);

            responseBytes?.Dispose();
            
            _disposed = true;
        }
    }
}