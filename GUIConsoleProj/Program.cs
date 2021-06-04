using System;
using Terminal.Gui;
using RepositoriesAndData;
using System.IO;
using System.Net.Sockets;
using System.Net;
using Service;

namespace GUIConsoleProj
{
    class Program
    {


        public static RemoteService remoteService;
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Loopback;
            int port = 3000;

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);  
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            Application.Init();

            
                try
                {
                    sender.Connect(remoteEP);
                    remoteService = new RemoteService(sender);
                }
                catch (Exception e) 
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
                LogInWindow log = new LogInWindow();
                log.SetLogWindow();
                Application.Run(log);

        }
    }
}
