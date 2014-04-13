using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceProxyImpl
{
    public interface IMessageGenerator
    {
        string GenerateRequestMessage(object requestData);
    }

    /// <summary>
    /// 报文生成器
    /// </summary>
    public abstract class MessageGenerator<TRequestData> : IMessageGenerator
        where TRequestData : RequestData
    {
        public static Encoding DefaultEncoding = Encoding.UTF8;

        public Encoding MessageEncoding { get; private set; }
        public abstract string MessageHeadTemplate { get; }

        public MessageGenerator()
        {
            this.MessageEncoding = DefaultEncoding;
        }

        public MessageGenerator(Encoding messageEncoding)
        {
            this.MessageEncoding = messageEncoding;
        }

        public virtual string GenerateRequestMessage(TRequestData requestData)
        {
            string messageBody = GenerateRequestBodyMessage(requestData);
            string messageHead = GenerateRequestHeadMessage(requestData, messageBody);
            return string.Format("{0}{1}{2}", messageHead, Environment.NewLine, messageBody);
        }

        protected virtual string GenerateRequestHeadMessage(TRequestData requestData, string bodyMessage)
        {
            return MessageHeadTemplate.Replace("$url", requestData.ServiceUri.AbsolutePath)
                                      .Replace("$host", requestData.ServiceUri.Host)
                                      .Replace("$method", requestData.MethodName)
                                      .Replace("$length", MessageEncoding.GetByteCount(bodyMessage).ToString());
        }
        protected abstract string GenerateRequestBodyMessage(TRequestData requestData);


        string IMessageGenerator.GenerateRequestMessage(object requestData)
        {
            return GenerateRequestMessage(requestData as TRequestData);
        }
    }
}