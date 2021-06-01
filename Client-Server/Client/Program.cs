using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Loopback;
            int port = 3000;
   
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);  
 
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            try 
            {
                sender.Connect(remoteEP);
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                while(true)
                {
                    Console.WriteLine("Enter command");
                    string command = Console.ReadLine();
                    if(command == "disconnect")
                    {
                        break;
                    }
                    else
                    {
                        byte[] commant = Encoding.ASCII.GetBytes(command);
                        sender.Send(commant);
                        byte[] buff = new byte[1024];
                        int nBytes = sender.Receive(buff);
                        string str = Encoding.ASCII.GetString(buff, 0, nBytes);
                        Console.WriteLine(str);
                    }
                }
                sender.Close();
            } 
            catch (Exception e) 
            {  
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }  

        }
    }
}
