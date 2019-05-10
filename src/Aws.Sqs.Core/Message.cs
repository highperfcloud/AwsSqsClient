using HighPerfCloud.Aws.Sqs.Core.Primitives;

namespace HighPerfCloud.Aws.Sqs.Core
{
    public sealed class LightweightMessage
    {
        //public static LightweightMessage Empty = new LightweightMessage();

        private LightweightMessage() { }

        public LightweightMessage(MessageId messageId)
        {
            MessageId = messageId;
        }

        public MessageId MessageId { get; }
    }
}