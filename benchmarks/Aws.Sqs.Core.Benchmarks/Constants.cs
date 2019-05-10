namespace Aws.Sqs.Core.Benchmarks
{
    internal class Constants
    {
        internal const string ResponseStart = @"<ReceiveMessageResponse><ReceiveMessageResult>";
        internal const string ResponseEnd = @"</ReceiveMessageResult><ResponseMetadata><RequestId>b6633655-283d-45b4-aee4-4e84e0ae6afa</RequestId></ResponseMetadata></ReceiveMessageResponse>";
        internal const string Message = @"<Message><MessageId>5fea7756-0ea4-451a-a703-a558b933e274</MessageId><ReceiptHandle>MbZj6wDWli=</ReceiptHandle><MD5OfBody>fafb00f5732ab283681e124bf8747ed1</MD5OfBody><Body>This is a test message</Body><Attribute><Name>SentTimestamp</Name><Value>1238099229000</Value></Attribute></Message>";
    }
}
