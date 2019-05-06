using System;

namespace HighPerfCloud.Aws.Sqs.Core.Primitives
{
    public readonly struct AwsRegion : IEquatable<AwsRegion>
    {
        // Asia Pacific
        public static readonly AwsRegion ApEast1 = new AwsRegion("ap-east-1");
        public static readonly AwsRegion ApNorthEast1 = new AwsRegion("ap-northeast-1");
        public static readonly AwsRegion ApNorthEast2 = new AwsRegion("ap-northeast-2");
        public static readonly AwsRegion ApNorthEast3 = new AwsRegion("ap-northeast-3");
        public static readonly AwsRegion ApSouth1 = new AwsRegion("ap-south-1");
        public static readonly AwsRegion ApSouthEast1 = new AwsRegion("ap-southeast-1");
        public static readonly AwsRegion ApSouthEast2 = new AwsRegion("ap-southeast-2");

        // Canada
        public static readonly AwsRegion CaCentral1 = new AwsRegion("ca-central-1");

        // China
        public static readonly AwsRegion CnCentral1 = new AwsRegion("cn-central-1");
        public static readonly AwsRegion CnNorth1 = new AwsRegion("cn-north-1");
        public static readonly AwsRegion CnNorthWest1 = new AwsRegion("cn-northwest-1");

        // Europe
        public static readonly AwsRegion EuCentral1 = new AwsRegion("eu-central-1");
        public static readonly AwsRegion EuNorth1 = new AwsRegion("eu-north-1");
        public static readonly AwsRegion EuWest1 = new AwsRegion("eu-west-1");
        public static readonly AwsRegion EuWest2 = new AwsRegion("eu-west-2");
        public static readonly AwsRegion EuWest3 = new AwsRegion("eu-west-3");

        // South America
        public static readonly AwsRegion SaEast1 = new AwsRegion("sa-east-1");

        // US
        public static readonly AwsRegion UsEast1 = new AwsRegion("us-east-1");
        public static readonly AwsRegion UsEast2 = new AwsRegion("us-east-2");
        public static readonly AwsRegion UsWest1 = new AwsRegion("us-west-1");
        public static readonly AwsRegion UsWest2 = new AwsRegion("us-west-2");

        private AwsRegion(string regionCode)
        {
            RegionCode = regionCode;
        }

        public string RegionCode { get; }

        public static bool operator ==(AwsRegion left, AwsRegion right) => Equals(left, right);

        public static bool operator !=(AwsRegion left, AwsRegion right) => !Equals(left, right);

        public override bool Equals(object obj) => (obj is AwsRegion awsRegion) && Equals(awsRegion);

        public bool Equals(AwsRegion other) => (RegionCode) == (other.RegionCode);

        public override int GetHashCode() => HashCode.Combine(RegionCode);
    }
}
