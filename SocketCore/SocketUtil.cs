using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace SocketCore
{
    public class SocketUtil
    {
        public const int BUFFER_SIZE = 1024;

        /// <summary>
        /// 建立Socket连接
        /// </summary>
        /// <param name="server">服务主机</param>
        /// <param name="port">端口号</param>
        /// <returns>返回Socket连接，如果建立失败则返回null</returns>
        public static Socket Connect(string server, int port)
        {
            Socket connectedSocket = null;
            IPHostEntry hostEntry = null;
 
            hostEntry = Dns.GetHostEntry(server);
          
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    tempSocket.Connect(ipe);
                    if (tempSocket.Connected)
                    {
                        connectedSocket = tempSocket;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (SocketException ex)
                { 
                    continue;
                }
            }
            return connectedSocket;
        }

        /// <summary>
        /// 监听端口
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="backlog">挂起连接队列的最大长度</param>
        /// <returns></returns>
        public static Socket Listen(int port, int backlog)
        { 
            Socket listenSocket = new Socket(AddressFamily.InterNetwork,
                                             SocketType.Stream,
                                             ProtocolType.Tcp);

            IPAddress hostIP = (Dns.Resolve(IPAddress.Any.ToString())).AddressList[0];
            IPEndPoint ep = new IPEndPoint(hostIP, port);
            listenSocket.Bind(ep);

            listenSocket.Listen(backlog);

            return listenSocket;
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socket">连接的Socket对象</param>
        /// <returns>返回消息字节数组</returns>
        public static byte[] Receive(Socket socket)
        {
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
            return messagePackageList.GetMessageBytes().ToArray();
        }
    }
}