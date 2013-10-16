using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public IDictionary<string, string> Header { get; private set; }

        public Request(Socket socket)
        {
            IDictionary<string, string> header = new Dictionary<string,string>();
            _socket = socket;

            NetworkStream stream = new NetworkStream(_socket);
            StreamReader reader = new StreamReader(stream);

            Url url = null;

            while (true)
            {
                string line = "";
                try
                {
                    line = reader.ReadLine();
                }
                catch (IOException e)
                {
                    Console.WriteLine("Error while reading request: " + e.Message);
                }
                
                if (string.IsNullOrEmpty(line))
                    break;

                if (url == null)
                {
                    var buffer = line.Split(' ');

                    url = new Url(buffer[0], buffer[1]);
                }
                else
                {
                    var parts = line.Split(": ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1)
                    {
                        header.Add(parts[0], parts[1]);
                    }
                }

                Console.WriteLine(line);
            }

            Header = new ReadOnlyDictionary<string, string>(header);

            if (url != null && url.Method == Url.MethodType.POST)
            {
                ProcessPostData();
            }
        }

        private void ProcessPostData()
        {
            throw new NotImplementedException();
        }

     }
}
