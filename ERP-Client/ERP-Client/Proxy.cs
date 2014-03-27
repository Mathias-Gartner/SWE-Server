using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ERP_Client
{
    public class Proxy
    {
        public Proxy(){}

        public List<Contact> KontaktSuchen()
        {
            WebClient client = new WebClient();
            //Uri baseUri = new Uri("http://localhost:8080");
            Uri baseUri = new Uri("http://10.128.241.99:8080");
            List<Contact> liste = new List<Contact>();

            string result = client.DownloadString(new Uri(baseUri, "?action=Erp&req=search"));

            liste = LoadFromXMLString(result);

            return liste;     //Noch schreiben XML to String Converter
        }

        public static List<Contact> LoadFromXMLString(string xmlText)
        {
            var stringReader = new StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(List<Contact>));
            return serializer.Deserialize(stringReader) as List<Contact>;
        }
    }
}
