using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Target
{
    class Program1
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No command specified\nSyntax example: weapon dir c:\\\nType: \"weapon disarm\" to close the target");
                Environment.Exit(1);
            }
            DateTime local;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener serverSocket = new TcpListener(ip, 8888); //Change port if necesary
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            local = DateTime.Now;
            Console.WriteLine(local.ToShortTimeString() + ": Opened server");
            clientSocket = serverSocket.AcceptTcpClient();
            local = DateTime.Now;
            Console.WriteLine(local.ToShortTimeString() + ": Connected to weapon");

            while (clientSocket.Connected)
            {
                StreamReader reader = new StreamReader(clientSocket.GetStream());
                StreamWriter writer = new StreamWriter(clientSocket.GetStream());
                writer.AutoFlush = true;
                string message = "";
                if (args.Length == 0)
                {
                    break;
                }
                for (int i = 0; i < args.Length; i++)
                {
                    message += args[i];
                    message += " ";
                }
                writer.WriteLine(message);
                string output = reader.ReadToEnd();
                Console.WriteLine(output);
                reader.Close();
                writer.Close();
                break;
            }
            serverSocket.Stop();
            local = DateTime.Now;
            Console.WriteLine(local.ToShortTimeString() + ": Closed server");
        }
    }
}
