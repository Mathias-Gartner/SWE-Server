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

        Uri baseUri = new Uri("http://localhost:8080");
        //Uri baseUri = new Uri("http://10.128.241.99:8080");
        //Uri baseUri = new Uri("http://10.201.92.108:8080");

        #region Kontakte
        public List<Contact> KontaktSuchen(Contact searchObject)
        {
            string xml = ToXmlString(searchObject);

            WebClient client = new WebClient();
            List<Contact> liste = new List<Contact>();            

            string result;

            try
            {
                result = client.UploadString(new Uri(baseUri, "?action=Erp&req=searchContact"), xml);
                liste = LoadFromXMLString(result);
            }
            catch (WebException)
            {
                result = string.Empty;
                liste = null;
            }
            catch (XmlException e)
            {
                result = e.Message;
                liste = null;
            }

            return liste;
        }

        public string KontaktChange(Contact changeObject)
        {
            string xml = ToXmlString(changeObject);
            string result;
            WebClient client = new WebClient();

            try
            {
                result = client.UploadString(new Uri(baseUri, "?action=Erp&req=saveContact"), xml);
            }
            catch (WebException w)
            {
                result = w.Message;
            }

            return result;
        }
        #endregion

        #region Rechnungen
        public void RechnungSuche()
        { }

        public void RechnungErstellen()
        { }
        #endregion

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
