using System;

namespace HighPerfCloud.Aws.Sqs.Core
{
    public readonly ref struct MessageBytesParser
    {
        private readonly ReadOnlySpan<byte> _messageBytes;

        public MessageBytesParser(ReadOnlySpan<byte> messageBytes)
        {
            // TODO: validate message has required elements

            _messageBytes = messageBytes;
        }

        /// <summary>
        /// Get the bytes representing the message ID.
        /// </summary>
        /// <returns>Returns the message ID as a <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</returns>
        public ReadOnlySpan<byte> GetMessageIdBytesSpan()
        {
            var startTagIndex = _messageBytes.IndexOf(SqsResponseConstants.MessageIdTagStart);
            var endTagIndex = _messageBytes.IndexOf(SqsResponseConstants.MessageIdTagEnd);

            var start = startTagIndex + SqsResponseConstants.MessageIdTagStart.Length;
            var length = endTagIndex - start;

            return _messageBytes.Slice(start, length);
        }
    }
}