using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Interface;

namespace SWE_Server
{
    class MyThread
    {       
        private Socket socket;

        public MyThread(Socket _socket)
        {
            socket = _socket;

            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        public void Run()
        {
            var request = new Request(socket);

            var data = PluginManager.getInstance().HandleRequest(request);

            var response = new Response(data);

            //Thread.Sleep(7500);       //zum testen der Multi-User-Fähigket
            response.send(socket);

            socket.Close();
        }
    }
}
