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
        private const string EmptyResponse = @"<ReceiveMessageResponse><ReceiveMessageResult></ReceiveMessageResult><ResponseMetadata><RequestId>b6633655-283d-45b4-aee4-4e84e0ae6afa</RequestId></ResponseMetadata></ReceiveMessageResponse>";

        private const string ResponseStart = @"<ReceiveMessageResponse><ReceiveMessageResult>";
        private const string ResponseEnd = @"</ReceiveMessageResult><ResponseMetadata><RequestId>b6633655-283d-45b4-aee4-4e84e0ae6afa</RequestId></ResponseMetadata></ReceiveMessageResponse>";
        private const string Message = @"<Message><MessageId>5fea7756-0ea4-451a-a703-a558b933e274</MessageId><ReceiptHandle>MbZj6wDWli=</ReceiptHandle><MD5OfBody>fafb00f5732ab283681e124bf8747ed1</MD5OfBody><Body>This is a test message</Body><Attribute><Name>SentTimestamp</Name><Value>1238099229000</Value></Attribute></Message>";

        [Fact]
        public void CountMessages_ShouldReturnZero_WhenResponseContainsEmptyBytes()
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
            for (var i = 0; i < messages; i++)
            {
                sb.Append(Message);
            }
            sb.Append(ResponseEnd);

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            var reader = new ReceiveMessageResponseReader();

            var count = reader.CountMessages(bytes);

            count.Should().Be(messages);
        }

        [Fact]
        public void CountMessages_ShouldNotReturnGreaterThanTen_EvenIfDataContainsMoreMessages()
        {
            var sb = new StringBuilder();
            sb.Append(ResponseStart);
            for (var i = 0; i < 15; i++) // response with 15 messages
            {
                sb.Append(Message);
            }
            sb.Append(ResponseEnd);

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            var reader = new ReceiveMessageResponseReader();

            var count = reader.CountMessages(bytes);

            count.Should().Be(10, because: "An AWS SQS receive request returns a maximum of 10 messages");
        }

        [Fact]
        public void TryGetNextMessageBytes_ShouldReturnAppropriateTuple_WhenResponseBytesContainsEmptyBytes()
        {
            var reader = new ReceiveMessageResponseReader();

            var messageFound = reader.TryGetNextMessageBytes(Array.Empty<byte>(), out var messageBytes, out var endPosition);

            var isDefault = messageBytes == default;

            messageFound.Should().BeFalse();
            isDefault.Should().BeTrue();                      
            endPosition.Should().Be(0);
        }

        [Fact]
        public void TryGetNextMessageBytes_ShouldReturnAppropriateTuple_WhenResponseBytesContainsNoMessage()
        {
            var bytes = Encoding.UTF8.GetBytes(EmptyResponse);

            var reader = new ReceiveMessageResponseReader();

            var messageFound = reader.TryGetNextMessageBytes(bytes, out var messageBytes, out var endPosition);

            var isDefault = messageBytes == default;

            messageFound.Should().BeFalse();
            isDefault.Should().BeTrue();
            endPosition.Should().Be(0);
        }

        [Fact]
        public void TryGetNextMessageBytes_ShouldReturnAppropriateTupleWithMessage_WhenResponseBytesContainsAMessage()
        {
            var sb = new StringBuilder();
            sb.Append(ResponseStart);
            sb.Append(Message);            
            sb.Append(ResponseEnd);

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            var reader = new ReceiveMessageResponseReader();

            var messageFound = reader.TryGetNextMessageBytes(bytes, out var messageBytes, out var endPosition);

            var messageSpanIsEqual = messageBytes.SequenceEqual(Encoding.UTF8.GetBytes(Message).AsSpan());

            messageFound.Should().BeTrue();
            messageSpanIsEqual.Should().BeTrue();
            endPosition.Should().Be(ResponseStart.Length + Message.Length);
        }      

        public static IEnumerable<object[]> OneToTen
        {
            get
            {
                for (var i = 1; i < 11; i++)
                {
                    yield return new object[] { i };
                }
            }
        }
    }
}
