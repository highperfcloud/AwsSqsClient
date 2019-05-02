
using System;

namespace HighPerfCloud.Aws.Sqs.Core
{
    internal static class SqsResponseConstants
    {
        public static ReadOnlySpan<byte> MessageTagStart => new byte[] { (byte)'<', (byte)'M', (byte)'e', (byte)'s', (byte)'s', (byte)'a', (byte)'g', (byte)'e', (byte)'>' };
        public static ReadOnlySpan<byte> MessageTagEnd => new byte[] { (byte)'<', (byte)'/', (byte)'M', (byte)'e', (byte)'s', (byte)'s', (byte)'a', (byte)'g', (byte)'e', (byte)'>' };
    }
}
