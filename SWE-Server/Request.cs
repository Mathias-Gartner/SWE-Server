using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Server
{
    class Request
    {
        private Socket _socket;

        public string method { get; private set; }

        public Request(Socket socket)
        {
            _socket = socket;            

            NetworkStream stream = new NetworkStream(_socket);
            StreamReader reader = new StreamReader(stream);

            while (true)
            {                
                string line = reader.ReadLine();
                
                if (string.IsNullOrEmpty(line))
                    break;
                
                if(string.IsNullOrEmpty(method))
                {
                    var buffer = line.Split(' ');

                    URL url = new URL(buffer[1]);
                }


                Console.WriteLine(line);

            }
        }


        
     }
}
