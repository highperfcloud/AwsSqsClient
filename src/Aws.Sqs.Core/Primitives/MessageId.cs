using System;
using System.Buffers.Text;

namespace HighPerfCloud.Aws.Sqs.Core.Primitives
{
    public readonly struct MessageId
    {
        public readonly Guid Value;

        internal MessageId(ReadOnlySpan<byte> messageIdBytes)
        {
            if (messageIdBytes.Length != 36 || !Utf8Parser.TryParse(messageIdBytes, out Value, out _))
            {
                throw new ArgumentException(message: "Invalid input", paramName:(nameof(messageIdBytes)));
            }
        }

        public override string ToString() => Value.ToString();
    }
}