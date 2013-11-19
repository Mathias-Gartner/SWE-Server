using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Interface;
using System.IO;
using System.Net;

namespace StaticFilePlugin
{
    public class StaticFilePlugin : IPlugin
    {
        private IReadOnlyDictionary<string, string> mimeTypes;

        public StaticFilePlugin()
        {
            Path = ConfigurationManager.AppSettings["DocumentRoot"];
            if (Path == null)
                Path = "";

            if (!Path.EndsWith("\\"))
                Path += "\\";

            var dictionary = new Dictionary<string, string>();
            dictionary.Add(".css", "text/css");
            dictionary.Add(".gif", "image/gif");
            dictionary.Add(".htm", "text/html");
            dictionary.Add(".html", "text/html");
            dictionary.Add(".jpg", "image/jpeg");
            dictionary.Add(".jpeg", "image/jpeg");
            dictionary.Add(".js", "text/javascript");
            dictionary.Add(".png", "image/png");
            dictionary.Add(".txt", "text/plain");
            mimeTypes = new ReadOnlyDictionary<string, string>(dictionary);
        }

        public string Path { get; set; }

        public string Name
        {
            get { return "StaticFile"; }
        }

        public string Author
        {
            get { return "Mathias Gartner"; }
        }

        public Data CreateProduct(Request request)
        {
            string filename = null;
            Data data = new Data();
            data.DocumentType = Data.DocumentTypeType.StandaloneFile;

            // dir up is not allowed, limit to DocumentRoot
            if (request.Url.Path.Contains(".."))
            {
                data.StatusCode = 403;
                return data;
            }

            var path = Path + request.Url.Path.Replace('/', '\\');
            var dir = new DirectoryInfo(path);
            var file = new FileInfo(path);
            if (file.Exists)
            {
                filename = file.Name;
                LoadFile(data, file);
            }
            else if (dir.Exists)
            {
                file = dir.GetFiles().Where(f => f.Name == "index.html").SingleOrDefault();
                if (file != null)
                {
                    filename = file.Name;
                    LoadFile(data, file);
                }
                else
                {
                    return DirectoryOverview(dir);
                }
            }
            else
            {
                return new Data() { StatusCode = 404 };
            }

            if (request.Url.Parameters.ContainsKey("download") && !String.IsNullOrEmpty(filename))
            {
                data.Disposition = filename;
            }

            return data;
        }

        public string GetMimeType(string extension)
        {
            if (mimeTypes.Keys.Contains(extension))
                return mimeTypes[extension];
            else
                return "application/octet-stream";
        }

        private void LoadFile(Data data, FileInfo file)
        {
            var memstream = new MemoryStream((int)file.Length);
            file.OpenRead().CopyTo(memstream);
            data.Content = memstream.ToArray();
            data.Contenttype = GetMimeType(file.Extension);
        }

        private Data DirectoryOverview(DirectoryInfo dir)
        {
            Data data = new Data();
            data.DocumentType = Data.DocumentTypeType.EmbeddedHtml;
            var sb = new StringBuilder();
            sb.Append("<div>");
            foreach (var subdir in dir.GetDirectories())
            {
                sb.Append("<p><a href=\"");
                sb.Append(WebUtility.HtmlEncode(subdir.Name));
                sb.Append("/\">");
                sb.Append(WebUtility.HtmlEncode(subdir.Name));
                sb.Append("/</a></p>");
            }
            foreach (var file in dir.GetFiles())
            {
                sb.Append("<p><a href=\"");
                sb.Append(WebUtility.HtmlEncode(file.Name));
                sb.Append("\">");
                sb.Append(WebUtility.HtmlEncode(file.Name));
                sb.Append("</a></p>");
            }
            data.SetContent(sb.ToString());
            return data;
        }
    }
}
