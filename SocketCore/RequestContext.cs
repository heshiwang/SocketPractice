using SocketCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace SocketCore
{
    /// <summary>
    /// 请求上下文
    /// </summary>
    public class RequestContext
    {
        private static Regex HostNameMatch = new Regex("Host:( )*([a-zA-Z0-9.]+)");

        public Encoding MessageEncoding { get; private set; }

        private string _requestMessage;
        /// <summary>
        /// 请求报文
        /// </summary>
        public string RequestMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_requestMessage))
                {
                    _requestMessage = MessageEncoding.GetString(RequestMessageByte);
                }
                return  _requestMessage;
            }
        }

        /// <summary>
        /// 请求的报文字节数据
        /// </summary>
        public byte[] RequestMessageByte { get; private set; }

        /// <summary>
        /// 请求的端口号
        /// </summary>
        public int Port { get; private set; }

        private string _requestMessageHostName;
        /// <summary>
        /// 请求报文中的主机名称
        /// </summary>
        public string RequestMessageHostName
        {
            get
            {
                if (string.IsNullOrEmpty(_requestMessageHostName))
                {
                    var match = HostNameMatch.Match(RequestMessage);
                    if (match.Success)
                    {
                        _requestMessageHostName = match.Groups[2].Value;
                    }
                    else
                    {
                        throw new Exception("在请求报文中获取主机号失败");
                    }
                }
                return _requestMessageHostName;
            }
        }

        /// <summary>
        /// 连接的Socket对象
        /// </summary>
        public Socket CurrentSocket { get; private set; }

        public RequestContext(Socket socket, int port)
            : this(socket, port, Encoding.UTF8)
        {
            this.CurrentSocket = socket;
        }

        public RequestContext(Socket socket, int port, Encoding messageEncoding)
        {
            this.CurrentSocket = socket;
            this.Port = port;
            this.MessageEncoding = messageEncoding;
            this.RequestMessageByte = SocketUtil.Receive(CurrentSocket); 
        }

        /// <summary>
        /// 请求报文是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return string.IsNullOrWhiteSpace(RequestMessage) == false;
        }
    }
}
