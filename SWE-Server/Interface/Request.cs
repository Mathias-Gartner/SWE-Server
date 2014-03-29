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
        public char[] RawPostData { get; private set; }

        public Request(Stream stream)
        {
            IDictionary<string, string> header = new Dictionary<string, string>();
            _stream = stream;

            if (stream == null)
                return;

            try
            {
                _reader = new StreamReader(stream, Encoding.UTF8, false, 4092, true);

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
                            var key = parts[0].ToLower().Trim();
                            var value = line.Substring(parts[0].Length + 1).ToLower().Trim();
                            header.Add(key, value);
                        }
                    }

                    Console.WriteLine(line);
                }

                Header = new ReadOnlyDictionary<string, string>(header);

                
                if (Header.ContainsKey("expect") && Header["expect"] == "100-continue")
                {
                    _reader.Close();
                    var writer = new StreamWriter(stream, Encoding.Default, 26, true);
                    writer.WriteLine("HTTP/1.1 100 Continue");
                    writer.WriteLine();
                    writer.Flush();
                    writer.Close();
                    _reader = new StreamReader(stream, Encoding.UTF8, false, 4092, true);
                }

                if (Url != null && Url.Method == Url.MethodType.POST)
                {
                    ProcessPostData();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Error receiving request: {0}\n{1}", e.Message, e.StackTrace);
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
            if (!Header.ContainsKey("content-length"))
                return;

            int length;
            if (!int.TryParse(Header["content-length"], out length))
                return; 
            
            char[] text = new char[length];
            var read = 0;
            while (read < length)
            {
                read += _reader.ReadBlock(text, read, length - read);
                //Console.WriteLine("post: " + new string(text));
            }

            RawPostData = text;
            
            if (!Header.ContainsKey("content-type"))
            {
                Console.WriteLine("Content-Type header is missing. Postdata is only available in RawPostData");
                return;
            }

            switch (Header["content-type"])
            {
                case "multipart/form-data":
                    throw new NotSupportedException();
                case "application/x-www-form-urlencoded":
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
                    Console.WriteLine("Unknown Content-Type. Postdata is only available in RawPostData");
                    break;
            }
        }
    }
}
