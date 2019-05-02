using FluentAssertions;
using HighPerfCloud.Aws.Sqs.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Aws.Sqs.Client.Tests
{
    public class ReceiveMessageResponseReaderTests
    {
        private static readonly string EmptyResponse = @"<ReceiveMessageResponse><ReceiveMessageResult></ReceiveMessageResult><ResponseMetadata><RequestId>b6633655-283d-45b4-aee4-4e84e0ae6afa</RequestId></ResponseMetadata></ReceiveMessageResponse>";

        private static readonly string ResponseStart = @"<ReceiveMessageResponse><ReceiveMessageResult>";
        private static readonly string ResponseEnd = @"</ReceiveMessageResult><ResponseMetadata><RequestId>b6633655-283d-45b4-aee4-4e84e0ae6afa</RequestId></ResponseMetadata></ReceiveMessageResponse>";
        private static readonly string Message = @"<Message><MessageId>5fea7756-0ea4-451a-a703-a558b933e274</MessageId><ReceiptHandle>MbZj6wDWli=</ReceiptHandle><MD5OfBody>fafb00f5732ab283681e124bf8747ed1</MD5OfBody><Body>This is a test message</Body><Attribute><Name>SentTimestamp</Name><Value>1238099229000</Value></Attribute></Message>";

        [Fact]
        public void CountMessages_ShouldReturnZero_WhenPassedEmptyBytes()
        {
            var reader = new ReceiveMessageResponseReader();

            var count = reader.CountMessages(Array.Empty<byte>());

            count.Should().Be(0);
        }

        [Fact]
        public void CountMessages_ShouldReturnZero_WhenResponseContainsNoMessages()
        {
            var bytes = Encoding.UTF8.GetBytes(EmptyResponse);

            var reader = new ReceiveMessageResponseReader();

            var count = reader.CountMessages(bytes);

            count.Should().Be(0);
        }
        
        [Theory]
        [MemberData(nameof(OneToTen))]
        public void CountMessages_ShouldReturnCorrectCount_WhenResponseContainsMessages(int messages)
        {
            var sb = new StringBuilder();
            sb.Append(ResponseStart);
            for (int i = 0; i < messages; i++)
            {
                sb.Append(Message);
            }
            sb.Append(ResponseEnd);

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            var reader = new ReceiveMessageResponseReader();

            var count = reader.CountMessages(bytes);

            count.Should().Be(messages);
        }

        public static IEnumerable<object[]> OneToTen
        {
            get
            {
                for (int i = 1; i < 11; i++)
                {
                    yield return new object[] { i };
                }
            }
        }
    }
}
