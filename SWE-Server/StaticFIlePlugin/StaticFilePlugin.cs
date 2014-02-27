using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Interface;
using System.IO;
using System.Net;
using System.Globalization;

namespace StaticFilePlugin
{
    public class StaticFilePlugin : IPlugin
    {
        private IReadOnlyDictionary<string, string> mimeTypes;

        public StaticFilePlugin()
        {
            Path = ConfigurationManager.AppSettings["DocumentRoot"];
            if (Path == null)
                Path = ".";

            Path = System.IO.Path.GetFullPath(Path);

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

            var requestPath = request.Url.Path.Replace("/", "\\");
            if (requestPath.StartsWith("\\"))
                requestPath = requestPath.Substring(1);

            var path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Path, requestPath));
            
            // dir up is not allowed, limit to DocumentRoot
            if (path.Length < Path.Length || path.Substring(0, Path.Length) != Path)
            {
                data.StatusCode = 403;
                return data;
            }

            var dir = new DirectoryInfo(path);
            var file = new FileInfo(path);
            if (file.Exists)
            {
                filename = file.Name;
                LoadFile(request, data, file);
            }
            else if (dir.Exists)
            {
                file = dir.GetFiles().Where(f => f.Name == "index.html").SingleOrDefault();
                if (file != null)
                {
                    filename = file.Name;
                    LoadFile(request, data, file);
                }
                else
                {
                    return DirectoryOverview(request, dir);
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

        private void LoadFile(Request request, Data data, FileInfo file)
        {
            if (IsNotModified(request, file))
            {
                data.StatusCode = 304;
            }
            else
            {
                var memstream = new MemoryStream((int)file.Length);
                file.OpenRead().CopyTo(memstream);
                data.Content = memstream.ToArray();
                data.Contenttype = GetMimeType(file.Extension);
                data.LastModified = file.LastWriteTimeUtc;
            }
        }

        private bool IsNotModified(Request request, FileInfo file)
        {
            if (request.Header.Keys.Contains("if-modified-since"))
            {
                bool parsed = true;
                DateTime date = DateTime.MinValue;
                var dateString = request.Header["if-modified-since"];
                try
                {
                    var day = int.Parse(dateString.Substring(5, 2));
                    var month = Array.FindIndex(
                        System.Globalization.DateTimeFormatInfo.InvariantInfo.AbbreviatedMonthNames
                            .Select(m => m.ToLower())
                            .ToArray(),
                        m => m == dateString.Substring(8, 3)
                        ) + 1;
                    var year = int.Parse(dateString.Substring(12, 4));
                    var hour = int.Parse(dateString.Substring(17, 2));
                    var minute = int.Parse(dateString.Substring(20, 2));
                    var second = int.Parse(dateString.Substring(23, 2));
                    date = new DateTime(year, month, day, hour, minute, second);
                }
                catch (ArgumentOutOfRangeException)
                {
                    parsed = false;
                }
                catch (FormatException)
                {
                    parsed = false;
                }

                if (parsed && date >= file.LastWriteTimeUtc)
                {
                    return true;
                }
            }
            return false;
        }

        private Data DirectoryOverview(Request request, DirectoryInfo dir)
        {
            Data data = new Data();
            data.DocumentType = Data.DocumentTypeType.EmbeddedHtml;
            var sb = new StringBuilder();
            sb.Append("<div>");
            foreach (var subdir in dir.GetDirectories())
            {
                sb.Append("<p><a href=\"");
                sb.Append(request.Url.Path);
                sb.Append("/");
                sb.Append(WebUtility.HtmlEncode(subdir.Name));
                sb.Append("/");
                if (request.Header.ContainsKey("download"))
                    sb.Append("?download");
                sb.Append("\">");
                sb.Append(WebUtility.HtmlEncode(subdir.Name));
                sb.Append("/</a></p>");
            }
            foreach (var file in dir.GetFiles())
            {
                sb.Append("<p><a href=\"");
                sb.Append(request.Url.Path);
                sb.Append("/");
                sb.Append(WebUtility.HtmlEncode(file.Name));
                if (request.Header.ContainsKey("download"))
                    sb.Append("?download");
                sb.Append("\">");
                sb.Append(WebUtility.HtmlEncode(file.Name));
                sb.Append("</a></p>");
            }
            sb.Append("</div>");
            data.SetContent(sb.ToString());
            return data;
        }
    }
}
