
//using HighPerfCloud.Aws.Sqs.Core.Primitives;
//using System;
//using System.Net.Http;
//using System.Net.Http.Headers;

//namespace HighPerfCloud.Aws.Sqs.Core
//{
//    internal struct AwsRequestBuilder
//    {


//        public HttpRequestMessage BuildReceiveMessage(in AwsRegion region, AccountId accountId, string queueName)
//        {

//        }


//        //public static DateTime InitializeHeaders(HttpRequestHeaders headers, Uri requestEndpoint, DateTime requestDateTime)
//        //{
   

//        //    if (!headers.ContainsKey(HeaderKeys.HostHeader))
//        //    {
//        //        var hostHeader = requestEndpoint.Host;
//        //        if (!requestEndpoint.IsDefaultPort)
//        //            hostHeader += ":" + requestEndpoint.Port;
//        //        headers.Add(HeaderKeys.HostHeader, hostHeader);
//        //    }

//        //    var dt = requestDateTime;
//        //    headers[HeaderKeys.XAmzDateHeader] = dt.ToUniversalTime().ToString(AWSSDKUtils.ISO8601BasicDateTimeFormat, CultureInfo.InvariantCulture);

//        //    return dt;
//        //}
//    }

//    public readonly struct SqsQueueReader
//    {
//        private readonly string _region;
//        private readonly string _accountNumber;
//        private readonly string _queueName;

//        public SqsQueueReader(in AwsRegion region, AccountId accountId, string queueName)
//        {
//            _region = region;
//            _accountNumber = accountNumber;
//            _queueName = queueName;
//        }

//        public void ReceieveSingleMessage()
//        {

//        }
//    }
//}
