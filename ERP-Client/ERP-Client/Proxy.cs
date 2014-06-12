using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace ERPClient
{
    public class Proxy
    {
        public Proxy() { }

        Uri baseUri = new Uri(Properties.Settings.Default.Serveradress);

        #region Kontakte
        public ICollection<Contact> KontaktSuchen(Contact searchObject)
        {
            string xml = ToXmlString(searchObject);
            ICollection<Contact> liste = new List<Contact>();            
            string result;

            using (var client = new WebClient())
            {
                try
                {
                    result = client.UploadString(new Uri(baseUri, "?action=Erp&req=searchContact"), xml);
                    liste = LoadFromXMLString<Contact>(result);
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
            }

            return liste;
        }

        public string KontaktChange(Contact changeObject)
        {
            string xml = ToXmlString(changeObject);
            string result;

            using (var client = new WebClient())
            {
                try
                {
                    result = client.UploadString(new Uri(baseUri, "?action=Erp&req=saveContact"), xml);
                }
                catch (WebException w)
                {
                    result = w.Message;
                }
            }

            return result;
        }
        #endregion

        #region Rechnungen

        public ICollection<Invoice> RechnungSuchen(Invoice searchObject)
        {
            string xml = ToXmlString(searchObject);
            ICollection<Invoice> liste = new List<Invoice>();
            string result;

            using (var client = new WebClient())
            {
                try
                {
                    result = client.UploadString(new Uri(baseUri, "?action=Erp&req=searchInvoice"), xml);
                    liste = LoadFromXMLString<Invoice>(result);
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
            }

            return liste;
        }

        public string RechnungChange(Invoice changeObject)
        {
            string xml = ToXmlString(changeObject);
            string result;

            using (var client = new WebClient())
            {
                try
                {
                    result = client.UploadString(new Uri(baseUri, "?action=Erp&req=saveInvoice"), xml);
                }
                catch (WebException w)
                {
                    result = w.Message;
                }
            }

            return result;
        }

        public string RechnungPdf(Invoice searchObject)
        {
            string xml = ToXmlString(searchObject);
            byte[] result;

            using (var client = new WebClient())
            {
                try
                {
                    result = client.UploadData(new Uri(baseUri, "?action=Erp&req=generateInvoiceDocument"), Encoding.UTF8.GetBytes(xml));
                }
                catch (WebException)
                {
                    return null;
                }
            }

            try
            {
                var file = Path.GetTempFileName();
                File.WriteAllBytes(file, result);
                return file;
            }
            catch (IOException)
            {
                return null;
            }
        }

        #endregion

        public static ICollection<T> LoadFromXMLString<T>(string xmlText)
        {
            using (var stringReader = new StringReader(xmlText))
            {
                var s = new XmlReaderSettings();
                s.CheckCharacters = true;
                s.ValidationType = ValidationType.None;
                var xmlReader = XmlReader.Create(stringReader, s); // XmlReader for validating xml as it throws nicer exceptions
                var serializer = new XmlSerializer(typeof(List<T>));
                return serializer.Deserialize(xmlReader) as List<T>;
            }
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
