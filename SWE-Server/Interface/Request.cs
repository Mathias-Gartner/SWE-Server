using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Request
    {
        private Stream _stream;
        private StreamReader _reader;

        public Url Url { get; private set; }
        public IReadOnlyDictionary<string, string> Header { get; private set; }
        public IReadOnlyDictionary<string, string> PostData { get; private set; }

        public Request(Stream stream)
        {
            IDictionary<string, string> header = new Dictionary<string, string>();
            _stream = stream;

            if (stream == null)
                return;

            try
            {
                _reader = new StreamReader(stream, Encoding.Default, false, 4092, true);

                while (true)
                {
                    string line = "";
                    try
                    {
                        line = _reader.ReadLine();
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("Error while reading request: {0}", e.Message);
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
                        var parts = line.Split(':');
                        if (parts.Length > 1)
                        {
                            header.Add(
                                parts[0].ToLower().Trim(),
                                line.Substring(parts[0].Length + 1).ToLower().Trim());
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
                Console.WriteLine("Error receiving request: {0}", e.Message);
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
                if (_reader != null)
                    _reader.Close();
            }
        }

        private void ProcessPostData()
        {
            if (!Header.ContainsKey("content-length") || !Header.ContainsKey("content-type"))
                return;

            switch (Header["content-type"])
            {
                case "multipart/form-data":
                    throw new NotSupportedException();
                case "application/x-www-form-urlencoded":
                    int length;
                    if (!int.TryParse(Header["content-length"], out length))
                        return;

                    char[] text = new char[length];
                    if (_reader.ReadBlock(text, 0, length) != length)
                        return;

                    string line = new string(text);
                    if (String.IsNullOrEmpty(line))
                        break;

                    line = WebUtility.UrlDecode(line);

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
