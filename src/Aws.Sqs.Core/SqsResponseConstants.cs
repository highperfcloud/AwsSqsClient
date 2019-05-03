
using System;

namespace HighPerfCloud.Aws.Sqs.Core
{
    internal static class SqsResponseConstants
    {
        public static byte OpenAngleBracket = (byte)'<';
        public static byte CloseAngleBracket = (byte)'>';
        public static byte ForwardSlash = (byte)'/';

        public static ReadOnlySpan<byte> MessageTag => new[] { (byte)'M', (byte)'e', (byte)'s', (byte)'s', (byte)'a', (byte)'g', (byte)'e' };

        public static ReadOnlySpan<byte> MessageTagStart => new[] { (byte)'<', (byte)'M', (byte)'e', (byte)'s', (byte)'s', (byte)'a', (byte)'g', (byte)'e', (byte)'>' };
        public static ReadOnlySpan<byte> MessageTagEnd => new[] { (byte)'<', (byte)'/', (byte)'M', (byte)'e', (byte)'s', (byte)'s', (byte)'a', (byte)'g', (byte)'e', (byte)'>' };
    }
}
