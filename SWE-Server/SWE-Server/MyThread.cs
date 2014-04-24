using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Interface;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using SWE_Server.Properties;
using log4net;

namespace SWE_Server
{
    class MyThread
    {       
        private Socket socket;
        private ILog logger;

        public MyThread(Socket _socket)
        {
            socket = _socket;
            logger = LogManager.GetLogger(GetType());
            logger.DebugFormat("New Thread for connection from {0} started", _socket.RemoteEndPoint.ToString());

            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        public void Run()
        {
            Stream stream = null;
            try
            {
                if (Settings.Default.UseHttps)
                {
                    var sslStream = new SslStream(new NetworkStream(socket));
                    var cert = new X509Certificate(Settings.Default.Certificate, Settings.Default.PrivateKeyPassword);
                    sslStream.AuthenticateAsServer(cert);
                    stream = sslStream;
                }
                else
                {
                    stream = new NetworkStream(socket);
                }
            }
            catch (IOException e)
            {
                logger.Error("Cannot create NetworkStream", e);
                return;
            }

            var request = new Request(stream);

            var data = PluginManager.getInstance().HandleRequest(request);

            var response = new Response(data);

            //Thread.Sleep(7500);       //zum testen der Multi-User-Fähigket
            response.send(stream);

            if (stream != null)
                stream.Close();
            socket.Close();
        }
    }
}
