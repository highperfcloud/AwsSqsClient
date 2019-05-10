using FluentAssertions;
using HighPerfCloud.Aws.Sqs.Core;
using System;
using System.Text;
using Xunit;

namespace Aws.Sqs.Core.Tests
{
    public class MessageBytesParserTests
    {
        private const string Message = @"<Message><MessageId>5fea7756-0ea4-451a-a703-a558b933e274</MessageId><ReceiptHandle>MbZj6wDWli=</ReceiptHandle><MD5OfBody>fafb00f5732ab283681e124bf8747ed1</MD5OfBody><Body>This is a test message</Body><Attribute><Name>SentTimestamp</Name><Value>1238099229000</Value></Attribute></Message>";
        private const string MessageId = "5fea7756-0ea4-451a-a703-a558b933e274";

        [Fact]
        public void GetMessageIdBytesSpan_ShouldReturnCorrectSpan()
        {
            var bytes = Encoding.UTF8.GetBytes(Message);
            var messageIdBytes = Encoding.UTF8.GetBytes(MessageId);

            var reader = new MessageBytesParser(bytes);

            var messageIdSpan = reader.GetMessageIdBytesSpan();

            var result = messageIdSpan.SequenceEqual(messageIdBytes);

            result.Should().BeTrue();
        }
    }
}
