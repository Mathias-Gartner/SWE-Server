using ErpPlugin.Data;
using ErpPlugin.Data.Database;
using Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ErpPlugin
{
    public class ErpPlugin : IPlugin
    {
        private const string c_namespace = "http://stud12.technikum-wien.at/~if12b007/SweErpXmlSchema.xsd";

        public ErpPlugin()
        {
            
        }

        public string Name
        {
            get { return "Erp"; }
        }

        public string Author
        {
            get { return "Mathias Gartner"; }
        }

        public virtual Interface.Data CreateProduct(Request request)
        {
            var data = new Interface.Data();
            data.Contenttype = "text/xml";
            data.DocumentType = Interface.Data.DocumentTypeType.StandaloneFile;

            if (request.Header.ContainsKey("content-type")
                && request.Header["content-type"] == "text/xml")
            {
                var memStream = new MemoryStream();
                memStream.Write(Encoding.UTF8.GetBytes(new string(request.RawPostData)), 0, request.RawPostData.Length);
                memStream.Seek(0, SeekOrigin.Begin);

                // Use a XmlReader for schema validation
                var validationSettings = new XmlReaderSettings();
                validationSettings.Schemas.Add(c_namespace, "ErpXmlSchema.xsd");
                validationSettings.ValidationEventHandler += XmlValidationErrorHandler;
                validationSettings.ValidationType = ValidationType.Schema;
                validationSettings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;

                var xmlReader = XmlReader.Create(memStream, validationSettings);
                var xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(xmlReader); // this also triggers schema validation

                    // validation succeeds if no element with erp namesapce is found, so check that too
                    if (!xmlDoc.ChildNodes.OfType<XmlNode>().Any(n => n.NamespaceURI == c_namespace))
                        throw new XmlSchemaValidationException("XML doesn't contains the required schema with namespace " + c_namespace);
                }
                catch (XmlException e)
                {
                    return PutErrorMessage(data, "<h1>Posted XML is invalid</h1>" + e.Message);
                }
                catch (XmlSchemaValidationException e)
                {
                    return PutErrorMessage(data, "<h1>Postdata schema valiation failed</h1>" + e.Message);
                }

                var rootElement = xmlDoc.FirstChild;
                var authElement = rootElement.ChildNodes.OfType<XmlNode>().Single(n => n.Name == "authentication");
                if (!Authenticated(authElement))
                {
                    return PutErrorMessage(data, "<h1>Invalid credentials</h1>", 401);
                }

                var element = rootElement.ChildNodes.OfType<XmlNode>().Except(new[] { authElement }).Single();
                switch (element.Name)
                {
                    case "search":
                        Search(data, element);
                        break;
                    case "save":
                        Save(data, element);
                        break;
                    case "delete":
                        Delete(data, element);
                        break;
                }
            }
            else
            {
                return PutErrorMessage(data, "<h1>XML Postdata required</h1>");
            }
            return data;
        }

        protected virtual void Search(Interface.Data data, XmlNode searchNode)
        {
            string errorMessage = string.Empty;
            IEnumerable<BusinessObject> result = null;
            var element = searchNode.ChildNodes[0];
            var instance = Deserialize(element);

            instance.State = BusinessObject.BusinessObjectState.SearchObject;
            if (instance is Contact)
                result = ((Contact)instance).Search();

            BuildResponse(data, result != null && result.Count() > 0 && string.IsNullOrEmpty(errorMessage), errorMessage, result);
        }

        protected virtual void Save(Interface.Data data, XmlNode saveNode)
        {
            string errorMessage = string.Empty;
            var element = saveNode.ChildNodes[0];
            var instance = Deserialize(element);

        }

        protected virtual void Delete(Interface.Data data, XmlNode deleteNode)
        {

        }
        
        protected virtual void BuildResponse(Interface.Data data, bool success, string errorMessage, IEnumerable<BusinessObject> results)
        {
            if (!success && String.IsNullOrEmpty(errorMessage))
                throw new ArgumentException("If the responce is indicating failure, a errorMessage is required");

            var memstream = new MemoryStream();
            var writer = new XmlTextWriter(memstream, Encoding.UTF8);
            writer.WriteStartDocument();
            writer.WriteStartElement("response", c_namespace);
            writer.WriteAttributeString("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", c_namespace + " SweErpXmlSchema.xsd");

            writer.WriteStartElement("status", c_namespace);
            writer.WriteString(success ? "success" : "error");
            writer.WriteEndElement();

            if (!String.IsNullOrEmpty(errorMessage))
            {
                writer.WriteStartElement("errormessage", c_namespace);
                writer.WriteString(errorMessage);
                writer.WriteEndElement();
            }

            if (results != null && results.Count() > 0)
            {
                writer.WriteStartElement("results", c_namespace);
                foreach (var businessObject in results)
                    Serialize(writer, businessObject);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            data.Content = memstream.ToArray();
        }

        protected void Serialize(XmlWriter writer, BusinessObject businessObject)
        {
            var xs = new XmlSerializer(businessObject.GetType());
            xs.Serialize(writer, businessObject);
        }

        protected BusinessObject Deserialize(XmlNode element)
        {
            var t = Type.GetType("ErpPlugin." + element.Name);
            var xs = new XmlSerializer(t);

            var writeStream = new MemoryStream();
            var memWriter = new XmlTextWriter(writeStream, Encoding.UTF8);
            memWriter.WriteStartDocument();
            memWriter.WriteStartElement("Contact");
            element.WriteContentTo(memWriter);
            memWriter.WriteEndElement();
            memWriter.WriteEndDocument();
            memWriter.Close();

            // strip wrong xmlns declarations
            var xml = Encoding.UTF8.GetString(writeStream.ToArray());
            xml = xml.Replace(string.Format(" xmlns=\"{0}\"", c_namespace), string.Empty);

            var readStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            return (BusinessObject)xs.Deserialize(readStream);
        }

        protected bool Authenticated(XmlNode authElement)
        {
            var searchUser = User.CreateSearchObject();
            searchUser.Username = authElement.ChildNodes.OfType<XmlNode>().Single(n => n.Name == "username").InnerText;

            var users = searchUser.Search();
            if (users == null || users.Count != 1)
                return false;

            var user = users.Single();
            var hash = User.CreatePasswordHash(authElement.ChildNodes.OfType<XmlNode>().Single(n => n.Name == "password").InnerText, user.PasswordSalt);
            return user.PasswordHash == hash;
        }

        protected Interface.Data PutErrorMessage(Interface.Data data, string message, int statuscode = 400)
        {
            data.StatusCode = statuscode;
            data.Contenttype = "text/html";
            data.SetContent(message);
            return data;
        }

        private void XmlValidationErrorHandler(object sender, ValidationEventArgs e)
        {
            throw e.Exception;
        }
    }
}
