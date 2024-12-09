using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //string host = Dns.GetHostName();
            string host = "127.0.0.1"; // For Ip.4
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Connector connector = new Connector();

            connector.Connect(endPoint, 
            () => { return SessionManager.Instance.Generate(); },
            10);

            while (true)
            {
                try
                {
                    SessionManager.Instance.SendForEach();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                }

                Thread.Sleep(250);
            }
        }
    }
}