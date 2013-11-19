using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Interface;

namespace SWE_Server
{
    class Response
    {
        private Data _data;

        public Response(Data data)
        {
            _data = data;
        }

        public void send(Stream stream)
        {
            if (stream == null || _data == null)
                return;

            try
            {
                var writer = new StreamWriter(stream);

                writer.Write("HTTP/1.1 ");

                switch (_data.StatusCode)
                {
                    case 200:
                        writer.WriteLine("200 OK");
                        break;
                    case 400:
                        writer.WriteLine("400 Bad Request");
                        if (_data.Content.Length < 1)
                            _data.SetContent("<h1>Bad Request</h1>");
                        break;
                    case 401:
                        writer.WriteLine("403 Forbidden");
                        if (_data.Content.Length < 1)
                            _data.SetContent("<h1>Forbidden</h1>");
                        break;
                    case 404:
                        writer.WriteLine("404 Not Found");
                        if (_data.Content.Length < 1)
                            _data.SetContent("<h1>Url not found</h1>");
                        break;
                    default:
                        writer.WriteLine("500 Internal Server Error");
                        if (_data.Content.Length < 1)
                            _data.SetContent("<h1>Internal Server Error</h1>");
                        break;
                }

                if (!String.IsNullOrEmpty(_data.Disposition))
                {
                    writer.WriteLine(String.Format("Content-Disposition: attachment; filename=\"{0}\"", _data.Disposition));
 
                }

                writer.WriteLine("Connection: close");
                writer.WriteLine("Content-Type: {0}", _data.Contenttype);
                
                writer.WriteLine("Content-Length: {0}", _data.Content.Length);
                writer.WriteLine();
                writer.Flush();

                if (_data.DocumentType == Data.DocumentTypeType.EmbeddedHtml)
                    appendFile(stream, Properties.Settings.Default.HeaderFile);

                stream.Write(_data.Content, 0, _data.Content.Length);

                if (_data.DocumentType == Data.DocumentTypeType.EmbeddedHtml)
                    appendFile(stream, Properties.Settings.Default.FooterFile);
            }
            catch (IOException e)
            {
                ExceptionHandler.ErrorMsg(3, e);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

        private void appendFile(Stream stream, string filename)
        {
            if (File.Exists(filename))
            {
                var fs = File.OpenRead(filename);
                fs.CopyTo(stream);
                fs.Close();
            }
        }
    }
}
