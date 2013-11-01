using System;
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
            PluginManager manager = PluginManager.getInstance();
            manager.LoadPlugins();
            
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            while (true)
            {
                Socket socket = listener.AcceptSocket();     //Wartet
                MyThread thread = new MyThread(socket);
            }
        }
    }
}
