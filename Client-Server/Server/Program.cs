using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
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
                    Socket clientSocket = listener.Accept();

                    Console.WriteLine($"Client connected: [{clientSocket.RemoteEndPoint.ToString()}]");

                    Thread thread = new Thread(StartNewTread);
                    thread.Start(clientSocket);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"can not connect to port: '{ex.Message}'");
            }
        }

        static void StartNewTread(object obj)
        {
            Socket socket = (Socket)obj;
            ProcessClient(socket);
            socket.Close();
            Console.WriteLine("Connection closed");
        }

        static void ProcessClient(Socket socket)
        {
            while(true)
            {
                byte[] buff = new byte[1024];
                int nByte = socket.Receive(buff);
                string resp = Encoding.ASCII.GetString(buff, 0, nByte);
                resp = resp.ToUpper();
                byte[] responce = Encoding.ASCII.GetBytes(resp);
                socket.Send(responce);
            }
        }
    }
}
