using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Url
    {
        [DefaultValue(undefined)]
        public enum MethodType
        {
            undefined,
            GET,
            POST
        }

        public MethodType Method { get; private set; }
        public string Path { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }

        public Url(string method, string url)
        {
            Method = (MethodType)Enum.Parse(typeof(MethodType), method, true);
            Parse(url);
        }

        private void Parse(string url)
        {
            Parameters = new Dictionary<string, string>();
            var parts = url.Split('?');
            Path = parts[0];

            if (parts.Length > 1)
            {
                foreach (var part in parts[1].Split('&'))
                {
                    var pair = part.Split('=');
                    if (pair.Length > 1)
                    {
                        Parameters.Add(pair[0], pair[1]);
                    }
                    else if (pair.Length == 1)
                    {
                        Parameters.Add(pair[0], String.Empty);
                    }
                }
            }
        }
    }
}
