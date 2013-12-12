﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interface;
using System.Threading;
using System.IO;
using System.Xml;
using System.Net;

namespace NaviPlugin
{
    public class NaviPlugin : IPlugin
    {
        object lockObject = new Object();
        bool _generating = false;
        Dictionary<string, ICollection<string>> streetMap = new Dictionary<string, ICollection<string>>();

        public string Name
        {
            get { return "Navi"; }
        }

        public string Author
        {
            get { return "Mathias Gartner"; }
        }

        public bool Generated
        {
            get
            {
                if (streetMap.Keys.Count > 0 && !Generating)
                    return true;
                return false;
            }
        }

        public bool Generating
        {
            get
            {
                bool value;
                lock (lockObject)
                {
                    value = _generating;
                }
                return value;
            }
            set // should only be set to true using SetGenerating
            {
                if (value == false)
                {
                    lock (lockObject)
                    {
                        _generating = value;
                    }
                }
            }
        }

        public bool SetGenerating()
        {
            lock (lockObject)
            {
                if (!_generating)
                {
                    _generating = true;
                    return true;
                }
                else return false;
            }
        }

        public Data CreateProduct(Request request)
        {
            if (Generating)
                return GeneratingMessage();

            if (request.Url.Parameters.Keys.Contains("generate"))
                return StartGeneration();

            if (request.PostData != null && request.PostData.ContainsKey("street") && request.PostData["street"].Length > 0)
                return FindCities(request.PostData["street"]);
            else if (request.PostData != null)
                return ValidationError();

            /*var streetmapping = new StringBuilder(10000000);
            foreach (var key in streetMap.Keys)
            {
                streetmapping.Append("<p>");
                streetmapping.Append(key);
                streetmapping.Append("</p><ul>");
                foreach (var city in streetMap[key])
                {
                    streetmapping.Append("<li>");
                    streetmapping.Append(city);
                    streetmapping.Append("</li>");
                }
                streetmapping.Append("</ul>");
            }*/

            const string generateLink = "<div><a href=\"/?action=Navi&generate=1\">Generieren</a></div>";
            const string form = "<div><form action=\"/?action=Navi\" method=\"POST\" accept-charset=\"UTF-8\"><input type=\"text\" name=\"street\" /><input type=\"submit\" value=\"Stadt suchen\" /></form></div>";
            Data data = new Data();
            if (Generated)
                data.SetContent(/*streetmapping.ToString() +*/ generateLink + form);
            else
                data.SetContent(generateLink);

            return data;
        }

        private Data GeneratingMessage()
        {
            Data data = new Data();
            data.SetContent("<p>Die Generierung der Kartendaten wird zur Zeit durchgef&uuml;hrt.</p><a href=\"?action=Navi\">Nochmal probieren</a>");
            return data;
        }

        private Data ValidationError()
        {
            Data data = new Data();
            data.SetContent("<p>Die Eingaben waren ung&uuml;ltig.</p>");
            return data;
        }

        private Data FindCities(string street)
        {
            Data data = new Data();
            StringBuilder sb = new StringBuilder();
            if (streetMap.ContainsKey(street.ToLower()))
            {
                sb.Append("<div><p>");
                sb.Append(WebUtility.HtmlEncode(street));
                sb.Append("</p><ul>");
                foreach (var city in streetMap[street.ToLower()])
                {
                    sb.Append("<li>");
                    sb.Append(WebUtility.HtmlEncode(city));
                    sb.Append("</li>");
                }
                sb.Append("</ul></div>");
            }
            else
            {
                sb.Append("<div><p>Die Straße <i>");
                sb.Append(WebUtility.HtmlEncode(street));
                sb.Append("</i> wurde nicht gefunden.</p></div>");
            }
            sb.Append("<p><a href=\"javascript:history:back();\">Zur&uuml;ck</a></p>");
            data.SetContent(sb.ToString());
            return data;
        }

        private Data StartGeneration()
        {
            Thread thread = new Thread(new ThreadStart(DoGeneration));
            thread.Start();
            Data data = new Data();
            data.SetContent("<p>Generierung gestartet.</p><a href=\"?action=Navi\">Weiter</a>");
            return data;
        }

        private void DoGeneration()
        {
            if (Generating || !SetGenerating())
                return;

            streetMap.Clear();

            using (var file = new FileStream(@"E:\FH\austria-latest.osm", FileMode.Open, FileAccess.Read, FileShare.Read, 512000))
            {
                using (var reader = new XmlTextReader(file))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "osm")
                        {
                            using (var osm = reader.ReadSubtree())
                            {
                                while (osm.Read())
                                {
                                    if (osm.NodeType == XmlNodeType.Element && (osm.Name == "node" || osm.Name == "way"))
                                    {
                                        ReadNode(osm.ReadSubtree());
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //sort
            var keys = streetMap.Keys.ToArray();
            foreach (var key in keys)
            {
                streetMap[key] = streetMap[key].OrderBy(s=>s).ToList();
            }

            Generating = false;
        }

        private void ReadNode(XmlReader node)
        {
            string city = "", street = "";
            while (node.Read())
            {
                if (node.NodeType == XmlNodeType.Element && node.Name == "tag")
                {
                    var key = node.GetAttribute("k");
                    var value = node.GetAttribute("v");
                    switch (key)
                    {
                        case "addr:city":
                            city = value;
                            break;
                        case "addr:street":
                            street = value;
                            break;
                    }
                }
            }
            if (!String.IsNullOrEmpty(city) && !String.IsNullOrEmpty(street))
            {
                street.ToLower();

                if (!streetMap.ContainsKey(street))
                    streetMap[street] = new List<string>();
                
                if (!streetMap[street].Contains(city))
                    streetMap[street].Add(city);
            }
        }
    }
}
