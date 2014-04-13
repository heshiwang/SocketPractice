using SocketCore;
using SocketMessageRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketMessageRouteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketListener listener = new SocketListener();
            RequestRouter router = new RequestRouter();
            router.AddRouteData("localhost", 20000, "localhost", 20001); 
            router.AddRouteData("localhost", 20001, "localhost", 20915); 
            listener.OnRequest += (sender, requestArgs) => { router.Route(requestArgs.RequestContext); };
            listener.Listen(20000); 
            listener.Listen(20001); 
            Console.WriteLine("启动完成");
            Console.ReadKey();
        }
    }
}
