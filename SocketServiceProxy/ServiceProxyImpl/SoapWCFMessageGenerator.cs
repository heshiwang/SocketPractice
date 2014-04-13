using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceProxyImpl
{
    public class SoapWCFMessageGenerator : MessageGenerator<WCFRequestData>
    {
        public static string SoapWCFMessageHeadTemplate = @"POST $url HTTP/1.1
Host: $host
Content-Type: text/xml; charset=utf-8
Content-Length: $length" + Environment.NewLine +
"SOAPAction: \"http://tempuri.org/$interface/$method\"" + Environment.NewLine;

        public static string SoapWCFMessageBodyTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" + 
"<soap:Body>" + 
"<$method xmlns=\"http://tempuri.org/\">" +
"$param" + 
"</$method>" + 
"</soap:Body>" + 
"</soap:Envelope>" ;

        public SoapWCFMessageGenerator()
            : base()
        {
        }

        public SoapWCFMessageGenerator(Encoding messageEncoding)
            : base(messageEncoding)
        {
        }

        #region MessageGenerator 实现

        public override string MessageHeadTemplate
        {
            get
            {
                return SoapWCFMessageHeadTemplate;
            }
        }

        protected override string GenerateRequestHeadMessage(WCFRequestData requestData, string bodyMessage)
        {
            return base.GenerateRequestHeadMessage(requestData, bodyMessage).Replace("$interface", requestData.ServiceInterfaceName);
        }

        protected override string GenerateRequestBodyMessage(WCFRequestData requestData)
        {
            string paramInfo = string.Join(Environment.NewLine, requestData.ParameterNameValues.Select(item => string.Format("<{0}>{1}</{0}>", item.Key, item.Value)));
            return SoapWCFMessageBodyTemplate.Replace("$method", requestData.MethodName)
                                             .Replace("$param", paramInfo);
        }

        #endregion
    }
}