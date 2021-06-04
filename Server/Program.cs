using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Service;

namespace Server
{
    class Program
    {
        static string dbFilePath = "../data/database.db";
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Loopback;
            int port = 3000;
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen();
                Console.WriteLine($"Server ready at port {port}");

                while(true)
                {
                    Console.WriteLine("Waiting for clients...");
                    if(System.IO.File.Exists(dbFilePath))
                    {
                        Socket clientSocket = listener.Accept();

                        Console.WriteLine($"Client connected: [{clientSocket.RemoteEndPoint.ToString()}]");

                        Thread thread = new Thread(StartNewTread);
                        thread.Start(clientSocket);
                    }
                    else
                    {
                        Console.Error.WriteLine("Could not find database");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Can not connect to port: '{ex.Message}'");
            }
        }

        static void StartNewTread(object obj)
        {
            try
            {
                Socket socket = (Socket)obj;
                ServerService service = new ServerService(dbFilePath);
                UIService uiService = new UIService(socket, service);
                uiService.OnService();
            }
            catch (Exception)
            {
                Console.WriteLine("Client disconnected");
            }
        }
    }
}
