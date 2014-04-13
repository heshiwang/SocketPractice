using SocketCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketCore
{
    public delegate void HandleRequest(object sender, RequestEventArgs args);

    public class SocketListener
    {
        public const int DEFAULT_BACKLOG = 100;
        public event HandleRequest OnRequest;

        public void Listen(int port)
        {
            Listen(port, DEFAULT_BACKLOG);
        }

        public void Listen(int port, int backLog)
        { 
            ThreadPool.QueueUserWorkItem(s =>
            {
                Socket listenSocket = SocketUtil.Listen(port, backLog);
                while (true)
                {
                    Socket connectSocket = listenSocket.Accept();
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        OnRequest(this, new RequestEventArgs(connectSocket, port));
                        if (connectSocket.Connected)
                        {
                            connectSocket.Disconnect(true);
                        }
                    });
                }
            });
        }
    }

    public class RequestEventArgs : EventArgs
    {
        public RequestContext RequestContext { get; private set; }

        public RequestEventArgs(Socket connectSocket, int port)
        {
            this.RequestContext = new RequestContext(connectSocket, port);
        }
    }
}
