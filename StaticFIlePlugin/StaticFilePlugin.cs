using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interface;
using System.IO;

namespace StaticFilePlugin
{
    public class StaticFilePlugin : IPlugin
    {
        private IReadOnlyDictionary<string, string> mimeTypes;

        public StaticFilePlugin()
        {
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
            Data data = new Data();
            data.DocumentType = Data.DocumentTypeType.StandaloneFile;

            // dir up is not allowed, limit to DocumentRoot
            if (request.Url.Path.Contains(".."))
            {
                data.StatusCode = 401;
                return data;
            }

            var path = ConfigurationManager.AppSettings["DocumentRoot"] + request.Url.Path.Replace('/', '\\');
            var dir = new DirectoryInfo(path);
            var file = new FileInfo(path);
            if (file.Exists)
            {
                LoadFile(data, file);
            }
            else if (dir.Exists && dir.GetFiles().Any(f => f.Name == "index.html"))
            {
                LoadFile(data, dir.GetFiles().Where(f => f.Name == "index.html").Single());
            }
            else
            {
                return new Data() { StatusCode = 404 };
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
    }
}
