using System;

namespace HighPerfCloud.Aws.Sqs.Core
{
    public ref struct ReceiveMessageResponseReader
    {
        private const int MaxMessages = 10; // max ten messages to a response

        /// <summary>
        /// Returns the count of messages contained within the UTF8 bytes of a ReceiveMessageResponse.
        /// </summary>
        /// <param name="bytes">A <see cref="ReadOnlySpan{T}"/> whose generic type argument is <see cref="byte"/> representing
        /// the content data received from the SQS API ReceiveMessageResponse.</param>
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
    }
}
