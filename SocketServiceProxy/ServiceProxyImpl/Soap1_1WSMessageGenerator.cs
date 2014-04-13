using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceProxyImpl
{
    public class Soap1_1WSMessageGenerator : MessageGenerator<RequestData>
    {
        public static string Soap1_1MessageHeadTemplate = @"POST $url HTTP/1.1
Host: $host
Content-Type: text/xml; charset=utf-8
Content-Length: $length" + Environment.NewLine +
"SOAPAction: \"http://tempuri.org/$method\"" + Environment.NewLine;

        public static string Soap1_1MessageBodyTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
"<soap:Body>" + 
"<$method xmlns=\"http://tempuri.org/\">" + 
"$param" + 
"</$method>" + 
"</soap:Body>" + 
"</soap:Envelope>";

        public Soap1_1WSMessageGenerator()
            : base()
        {
        }

        public Soap1_1WSMessageGenerator(Encoding messageEncoding)
            : base(messageEncoding)
        {
        }

        #region MessageGenerator 实现

        public override string MessageHeadTemplate
        {
            get
            {
                return Soap1_1MessageHeadTemplate;
            }
        }

        protected override string GenerateRequestBodyMessage(RequestData requestData)
        {
            string paramInfo = string.Join(Environment.NewLine, requestData.ParameterNameValues.Select(item => string.Format("<{0}>{1}</{0}>", item.Key, item.Value)));
            return Soap1_1MessageBodyTemplate.Replace("$method", requestData.MethodName)
                                             .Replace("$param", paramInfo);
        }

        #endregion
    }
}