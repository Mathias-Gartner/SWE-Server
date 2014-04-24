using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Interface;
using SWE_Server.Properties;
using log4net;

namespace SWE_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetLogger(typeof(Program));
            int port = Settings.Default.Port;
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
                logger.Fatal("Error starting server", e);
                Environment.Exit(-1);
            }

            logger.InfoFormat("Server started on port {0}", port);

            while (true)
            {
                Socket socket = listener.AcceptSocket();     //Wartet
                logger.InfoFormat("Client connected from {0}", socket.RemoteEndPoint.ToString());
                MyThread thread = new MyThread(socket);
            }
        }
    }
}
