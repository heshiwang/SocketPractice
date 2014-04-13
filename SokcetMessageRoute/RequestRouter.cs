using SocketCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SocketMessageRoute
{
    /// <summary>
    /// 请求路由处理
    /// </summary>
    public class RequestRouter
    {
        public List<RouteData> RouteTable { get; private set; }

        public RequestRouter()
        {
            this.RouteTable = new List<RouteData>();
        }

        public void Route(RequestContext requestContext)
        {
            if (requestContext.CurrentSocket.Connected && requestContext.IsValid())
            {
                RouteData routeData = FindMatchRouteData(requestContext);
                if (routeData != null)
                {
                    Socket connectSocket = SocketUtil.Connect(routeData.TargetHostName, routeData.TargetPort);
                    connectSocket.Send(requestContext.RequestMessageByte);
                    byte[] recieveMessage = SocketUtil.Receive(connectSocket);
                    requestContext.CurrentSocket.Send(recieveMessage);
                }
            }
        }

        public void AddRouteData(string sourceHostName, int sourcePort, string targetHostName, int targetPort)
        {
            this.RouteTable.Add(new RouteData() { SourceHostName = sourceHostName, SourcePort = sourcePort, TargetHostName = targetHostName, TargetPort = targetPort });
        }

        private RouteData FindMatchRouteData(RequestContext requestContext)
        {
            return this.RouteTable.SingleOrDefault(item => item.SourceHostName.Equals(requestContext.RequestMessageHostName, StringComparison.CurrentCultureIgnoreCase) && item.SourcePort == requestContext.Port);
        }
    }
}
