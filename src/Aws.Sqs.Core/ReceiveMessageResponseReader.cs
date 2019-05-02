using System;

namespace HighPerfCloud.Aws.Sqs.Core
{
    public ref struct ReceiveMessageResponseReader
    {
        private const int MaxMessages = 10; // max ten messages to a response

        /// <summary>
        /// Returns the count of messages contained within the UTF8 bytes of a ReceiveMessageResponse.
        /// </summary>
        /// <param name="bytes">A <see cref="ReadOnlySpan{byte}"/> representing the data received from the SQS API ReceiveMessageResponse.</param>
        /// <returns>A count of the messages found in the byte data.</returns>
        public int CountMessages(in ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length == 0)
                return 0;
                       
            var lengthOfStartTag = SqsResponseConstants.MessageTagStart.Length;

            var count = 0;
            var remainingData = bytes;

            while(count <= MaxMessages) 
            {               
                var index = remainingData.IndexOf(SqsResponseConstants.MessageTagStart);

                if (index == -1)
                    break;

                remainingData = remainingData.Slice(index + lengthOfStartTag);
                count++;
            }

            return count;
        }
    }
}
