using System;

namespace HighPerfCloud.Aws.Sqs.Core
{
    public readonly struct ReceiveMessageResponseReader
    {
        private const int MaxMessages = 10; // max ten messages to a response

        /// <summary>
        /// Returns the count of messages contained within the UTF8 bytes of a ReceiveMessageResponse.
        /// </summary>
        /// <param name="bytes">The content data received from the SQS API ReceiveMessageResponse as a <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
        /// <returns>A count of the messages found in the byte data.</returns>
        public int CountMessages(in ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length == 0)
                return 0;

            var count = 0;
            var remainingData = bytes;
            int index;

            while (count < MaxMessages && (index = remainingData.IndexOf(SqsResponseConstants.MessageTagStart)) != -1)
            {
                remainingData = remainingData.Slice(index + SqsResponseConstants.MessageTagStart.Length);
                count++;
            }

            return count;
        }

        /// <summary>
        /// Attempt to parse a <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> for a message.
        /// </summary>
        /// <param name="responseBytes">A a <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> from an AWS received message response.</param>
        /// <param name="messageBytes">A a <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> over the message portion of the original response bytes.</param>
        /// <param name="endPosition">The position of the end of the message including its closing tag.</param>
        /// <returns>A <see cref="bool"/> indicating whether a message was found in the byte data.</returns>
        public bool TryGetNextMessageBytes(in ReadOnlySpan<byte> responseBytes, out ReadOnlySpan<byte> messageBytes, out int endPosition)
        {
            int messageStartTagIndex, messageEndTagIndex;

            if (responseBytes.Length == 0 || (messageStartTagIndex = responseBytes.IndexOf(SqsResponseConstants.MessageTagStart)) == -1 || (messageEndTagIndex = responseBytes.IndexOf(SqsResponseConstants.MessageTagEnd)) == -1)
            {
                messageBytes = default;
                endPosition = 0;

                return false;
            }

            endPosition = messageEndTagIndex + SqsResponseConstants.MessageTagEnd.Length;
            messageBytes = responseBytes[messageStartTagIndex..endPosition];

            return true;
        }
    }
}
