using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace SocketCore
{
    public class SocketProxy
    {
        public readonly static Encoding DefaultEncoding = Encoding.UTF8;

        public Uri ServiceUri { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        public SocketProxy(Uri serviceUri)
            : this(serviceUri, DefaultEncoding)
        {

        }

        public SocketProxy(Uri serviceUri, Encoding messageEncoding)
        {
            this.ServiceUri = serviceUri;
            this.MessageEncoding = messageEncoding;
        }

        /// <summary>
        /// 同步发送消息后返回接收的消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string Send(string message)
        {
            Socket socket = SocketUtil.Connect(ServiceUri.Host, ServiceUri.Port);
            if (socket != null)
            {
                try
                {
                    byte[] messageByte = MessageEncoding.GetBytes(message);
                    socket.Send(messageByte);
                    byte[] recieveMessage = SocketUtil.Receive(socket);
                    return MessageEncoding.GetString(recieveMessage);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("同步发送报文出现异常:{0}", ex.Message));
                    throw;
                }
                finally
                {
                    socket.Close();
                }
            }
            else
            {
                throw new Exception(string.Format("无法为地址{0}建立Socket连接", ServiceUri.ToString()));
            }
        }
    }
}