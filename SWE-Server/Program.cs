﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Interface;

namespace SWE_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 8080;
            PluginManager manager = PluginManager.getInstance();
            manager.LoadPlugins();
            
            TcpListener listener = new TcpListener(IPAddress.IPv6Any, port);
            // enable dual stacked server
            listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            try
            {
                listener.Start();
            }
            catch (SocketException e)
            {
                Console.WriteLine("FATAL: Error starting server: {0}", e.Message);
                Environment.Exit(-1);
            }

            Console.WriteLine("Server started on port {0}", port);

            while (true)
            {
                Socket socket = listener.AcceptSocket();     //Wartet
                Console.WriteLine("Client connected from {0}", socket.RemoteEndPoint.ToString());
                MyThread thread = new MyThread(socket);
            }
        }
    }
}
