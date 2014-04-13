using ServiceProxyImpl;
using SocketCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SocketProxyTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri serviceUri = new Uri(@"http://localhost:20915/CalculateService.asmx");

            //RequestData requestData = new RequestData(serviceUri, "AddReturnOutResult");
            RequestData requestData = new RequestData(serviceUri, "Add");
            requestData.AddParam("num1", "11");
            requestData.AddParam("num2", "11");

            try
            {
                CallServiceInHttpPost(requestData);
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------");
                CallServiceInSoap1_1(requestData);
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------");
                CallServiceInSoap1_2(requestData);
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------");
                CallWcfService();

            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        private static void CallWcfService()
        {
            Uri serviceUri = new Uri(@"http://localhost:20915/CalculateInWcf.svc");
            //WCFRequestData wcfRequestData = new WCFRequestData(serviceUri, "AddReturnOutResult", "ICalculate");
            WCFRequestData wcfRequestData = new WCFRequestData(serviceUri, "Add", "ICalculate");
            wcfRequestData.AddParam("num1", "11");
            wcfRequestData.AddParam("num2", "11");

            SoapWCFMessageGenerator wcfMessageGenerator = new SoapWCFMessageGenerator();
            SocketProxy proxy = new SocketProxy(wcfRequestData.ServiceUri);
            string requestMessage = wcfMessageGenerator.GenerateRequestMessage(wcfRequestData);
            Console.WriteLine("SOAP1.1协议请求WCF服务的报文:");
            Console.WriteLine(requestMessage);
            Console.WriteLine();
            Console.WriteLine("响应报文：");
            Console.WriteLine(proxy.Send(requestMessage));
        }

        private static void CallServiceInHttpPost(RequestData requestData)
        {
            HttpPostWSMessageGenerator httpPostMessageGenerator = new HttpPostWSMessageGenerator();
            SocketProxy proxy = new SocketProxy(requestData.ServiceUri);
            string requestMessage = httpPostMessageGenerator.GenerateRequestMessage(requestData);
            Console.WriteLine("HTTP POST协议请求WebService服务的报文:");
            Console.WriteLine(requestMessage);
            Console.WriteLine();
            Console.WriteLine("响应报文：");
            Console.WriteLine(proxy.Send(requestMessage));
        }

        private static void CallServiceInSoap1_1(RequestData requestData)
        {
            Soap1_1WSMessageGenerator soap1_1RequestMessage = new Soap1_1WSMessageGenerator();
            SocketProxy proxy = new SocketProxy(requestData.ServiceUri);
            string requestMessage = soap1_1RequestMessage.GenerateRequestMessage(requestData);
            Console.WriteLine("SOAP 1.1协议请求WebService服务的报文:");
            Console.WriteLine(requestMessage);
            Console.WriteLine();
            Console.WriteLine("响应报文：");
            Console.WriteLine(proxy.Send(requestMessage));
        }

        private static void CallServiceInSoap1_2(RequestData requestData)
        {
            Soap1_2WSMessageGenerator soap1_2RequestMessage = new Soap1_2WSMessageGenerator();
            SocketProxy proxy = new SocketProxy(requestData.ServiceUri);
            string requestMessage = soap1_2RequestMessage.GenerateRequestMessage(requestData);
            Console.WriteLine("SOAP 1.2协议请求WebService服务的报文:");
            Console.WriteLine(requestMessage);
            Console.WriteLine();
            Console.WriteLine("响应报文：");
            Console.WriteLine(proxy.Send(requestMessage));
        }
    }
}
