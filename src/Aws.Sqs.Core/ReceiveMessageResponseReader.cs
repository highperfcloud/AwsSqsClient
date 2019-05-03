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

        [Obsolete("This version benchmarked slower than the original")]
        internal int CountMessagesV2(in ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length == 0)
                return 0;
            
            var count = 0;
            var remainingData = bytes;
            int openAnglePosition;

            while (count < MaxMessages && (openAnglePosition = remainingData.IndexOf(SqsResponseConstants.OpenAngleBracket)) != -1)
            {
                var closeAnglePosition = remainingData.IndexOf(SqsResponseConstants.CloseAngleBracket);

                if (closeAnglePosition == -1)
                    break; // invalid XML

                if (remainingData[openAnglePosition + 1] != SqsResponseConstants.ForwardSlash) // not a closing tag '</'
                {
                    var lengthOfTag = closeAnglePosition - openAnglePosition - 1;

                    if (lengthOfTag == SqsResponseConstants.MessageTag.Length)
                    {
                        var tag = remainingData.Slice(openAnglePosition + 1, lengthOfTag);

                        if (tag.SequenceEqual(SqsResponseConstants.MessageTag))
                        { 
                            count++;
                        }
                    }
                }

                remainingData = remainingData.Slice(closeAnglePosition + 1);
            }

            return count;
        }
    }
}
