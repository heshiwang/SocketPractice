using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceProxyImpl
{
    public class Soap1_2WSMessageGenerator : MessageGenerator<RequestData>
    {
        public static string Soap1_2MessageHeadTemplate = @"POST $url HTTP/1.1
Host: $host
Content-Type: application/soap+xml; charset=utf-8
Content-Length: $length
";

        public static string Soap1_1MessageBodyTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + 
"<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">" + 
"<soap12:Body>" + 
"<$method xmlns=\"http://tempuri.org/\">" + 
"$param" + 
"</$method>" +
"</soap12:Body>" +
"</soap12:Envelope>";

        public Soap1_2WSMessageGenerator()
            : base()
        {
        }

        public Soap1_2WSMessageGenerator(Encoding messageEncoding)
            : base(messageEncoding)
        {
        }

        #region MessageGenerator 实现

        public override string MessageHeadTemplate
        {
            get
            {
                return Soap1_2MessageHeadTemplate;
            }
        }

        protected override string GenerateRequestBodyMessage(RequestData requestData)
        {
            string paramDesc = string.Join(Environment.NewLine, requestData.ParameterNameValues.Select(item => string.Format("<{0}>{1}</{0}>", item.Key, item.Value)));
            return Soap1_1MessageBodyTemplate.Replace("$method", requestData.MethodName)
                                             .Replace("$param", paramDesc);
        }

        #endregion
    }
}