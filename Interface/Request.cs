using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Request
    {
        private Socket _socket;
        private StreamReader _reader;

        public Url Url { get; private set; }
        public IReadOnlyDictionary<string, string> Header { get; private set; }
        public IReadOnlyDictionary<string, string> PostData { get; private set; }

        public Request(Socket socket)
        {
            IDictionary<string, string> header = new Dictionary<string, string>();
            _socket = socket;

            NetworkStream stream = null;
            try
            {
                stream = new NetworkStream(_socket);
                _reader = new StreamReader(stream);

                while (true)
                {
                    string line = "";
                    try
                    {
                        line = _reader.ReadLine();
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("Error while reading request: " + e.Message);
                    }

                    if (string.IsNullOrEmpty(line))
                        break;

                    if (Url == null)
                    {
                        var buffer = line.Split(' ');

                        Url = new Url(buffer[0], buffer[1]);
                    }
                    else
                    {
                        var parts = line.Split(": ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length > 1)
                        {
                            header.Add(parts[0].ToLower(), parts[1].ToLower());
                        }
                    }

                    Console.WriteLine(line);
                }

                Header = new ReadOnlyDictionary<string, string>(header);

                if (Url != null && Url.Method == Url.MethodType.POST)
                {
                    ProcessPostData();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Error receiving request: " + e.Message);
            }
            catch (NotSupportedException)
            {
                if (Url != null && Url.Method == Url.MethodType.POST)
                    Console.WriteLine("Content-Type not supported");
                else
                    Console.WriteLine("Request not supported");
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

        private void ProcessPostData()
        {
            switch (Header["content-type"])
            {
                case "multipart/form-data":
                    throw new NotSupportedException();
                case "application/x-www-form-urlencoded":
                    string line;
                    line = _reader.ReadLine();
                    if (String.IsNullOrEmpty(line))
                        break;

                    var dictionary = new Dictionary<string, string>();
                    var args = line.Split('&');
                    foreach (var arg in args)
                    {
                        var parts = arg.Split('=');
                        if (parts.Length < 2)
                            continue;
                        dictionary.Add(parts[0], parts[1]);
                    }
                    PostData = new ReadOnlyDictionary<string, string>(dictionary);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
