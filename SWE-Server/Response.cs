using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Server
{
    class Response
    {
        public void send(Socket socket)
        {
            NetworkStream stream = null;
            try
            {
                stream = new NetworkStream(socket);
                var writer = new StreamWriter(stream);

                writer.WriteLine("HTTP/1.1 200 OK");
                writer.WriteLine("Connection: close");
                writer.WriteLine("Content-Type: image/jpeg");
                //writer.WriteLine("Content-Type: text/html");

                var fs = File.OpenRead(@"C:\Users\Christoph\Pictures\Ich2.jpg");
                //var fs = File.OpenRead(@"E:\lvplan.html");

                writer.WriteLine("Content-Length: " + fs.Length);
                writer.WriteLine();
                writer.Flush();

                fs.CopyTo(stream);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error sending response: " + e.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }
    }
}
