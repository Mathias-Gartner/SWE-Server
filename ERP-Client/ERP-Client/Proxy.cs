using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ERP_Client
{
    public class Proxy
    {
        public Proxy() { }

        public List<Contact> KontaktSuchen(Contact searchObject)
        {
            string xml = ToXmlString(searchObject);

            WebClient client = new WebClient();
            Uri baseUri = new Uri("http://localhost:8080");
            //Uri baseUri = new Uri("http://10.128.241.99:8080");
            List<Contact> liste = new List<Contact>();            

            string result;
            
            try
            {
                result = client.UploadString(new Uri(baseUri, "?action=Erp&req=searchContact"), xml);
            }
            catch (WebException)
            {
                result = string.Empty;
            }

            try
            {
                liste = LoadFromXMLString(result); 
            }
            catch (XmlException)
            {
                // just leave list empty for now
            }

            return liste;
        }

        public static List<Contact> LoadFromXMLString(string xmlText)
        {
            var stringReader = new StringReader(xmlText);
            var s = new XmlReaderSettings();
            s.CheckCharacters = true;
            s.ValidationType = ValidationType.None;
            var xmlReader = XmlReader.Create(stringReader, s); // XmlReader for validating xml as it throws nicer exceptions
            var serializer = new XmlSerializer(typeof(List<Contact>));
            return serializer.Deserialize(xmlReader) as List<Contact>;
        }

        public static string ToXmlString(object obj)
        {
            if (obj == null) { throw new ArgumentNullException("obj"); }

            XmlSerializer xml = new XmlSerializer(obj.GetType());
            StringBuilder sb = new StringBuilder();
            xml.Serialize(XmlWriter.Create(sb), obj);
            return sb.ToString();
        }
    }
}
