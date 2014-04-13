using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceProxyImpl
{
    /// <summary>
    /// HTTP POST协议报文生成器
    /// </summary>
    public class HttpPostWSMessageGenerator : MessageGenerator<RequestData>
    {
        public const string HttpPostMessageHeadTemplate = @"POST $url/$method HTTP/1.1
Host: $host
Content-Type: application/x-www-form-urlencoded
Content-Length: $length
";

        public HttpPostWSMessageGenerator()
            : base()
        {
        }

        public HttpPostWSMessageGenerator(Encoding messageEncoding)
            : base(messageEncoding)
        {
        } 

        #region MessageGenerator 实现

        public override string MessageHeadTemplate
        {
            get
            {
                return HttpPostMessageHeadTemplate;
            }
        }

        protected override string GenerateRequestBodyMessage(RequestData requestData)
        {
            return string.Join("&", requestData.ParameterNameValues.Select(item => string.Format("{0}={1}", item.Key, item.Value)));
        }

        #endregion
    }
}