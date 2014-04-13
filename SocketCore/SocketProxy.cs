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
        public const int BUFFER_SIZE = 1024;
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
            Socket socket = GetSocket();
            try
            {
                byte[] messageByte = MessageEncoding.GetBytes(message);
                socket.Send(messageByte);

                SocketMessagePackageCollection messagePackageList = new SocketMessagePackageCollection();
                byte[] receiveBuffer = new byte[BUFFER_SIZE];
                while (true)
                {
                    int realReceiveLength = socket.Receive(receiveBuffer);
                    messagePackageList.AddPackage(new SocketMessagePackage(DateTime.Now, receiveBuffer, realReceiveLength));
                    if (socket.Available == 0 || realReceiveLength < receiveBuffer.Length)
                    {
                        break;
                    }
                }
                byte[] recieveMessage = messagePackageList.GetMessageBytes().ToArray();
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

        private Socket GetSocket()
        {
            IPHostEntry iphe = Dns.GetHostEntry(ServiceUri.Host);
            IPAddress[] addList = iphe.AddressList;
            foreach (IPAddress address in iphe.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, ServiceUri.Port);
                Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tempSocket.Connect(ipe);
                if (tempSocket.Connected)
                {
                    return tempSocket;
                }
                else
                {
                    continue;
                }
            }
            throw new Exception(string.Format("无法为地址{0}建立Socket连接", ServiceUri.ToString()));
        } 
    }
}