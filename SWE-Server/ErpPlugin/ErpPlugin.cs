using ErpPlugin.Data;
using ErpPlugin.Data.Database;
using ErpPlugin.Xml;
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

        public virtual Interface.Data CreateProduct(Interface.Request request)
        {
            var data = new Interface.Data();
            data.Contenttype = "text/xml";
            data.DocumentType = Interface.Data.DocumentTypeType.StandaloneFile;

            if (request.RawPostData == null || request.RawPostData.Count() < 1)
            {
                return PutErrorMessage(data, "<h1>Postdata required</h1>");
            }

            /*var xmlDoc = new XmlDocument();
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
            }*/

            //var xmlObject = request.RawPostData.FromXmlString<BusinessObject>();

            object result = null;
            try
            {
                switch (request.Url.Parameters["req"])
                {
                    case "searchContact":
                        var contact = request.RawPostData.FromXmlString<Contact>();
                        result = contact.Search();
                        break;
                    case "saveContact":
                        contact = request.RawPostData.FromXmlString<Contact>();
                        //result = contact.Save();
                        break;
                    case "deleteContact":
                        contact = request.RawPostData.FromXmlString<Contact>();
                        //result = contact.Delete();
                        break;
                }
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Contains("error in XML"))
                    return PutErrorMessage(data, "<h1>Error in XML</h1>");
                throw;
            }

            if (result != null)
                data.SetContent(result.ToXmlString());

            return data;
        }

        /*protected virtual void Search(Interface.Data data, BusinessObject searchObject)
        {
            string errorMessage = string.Empty;
            IEnumerable<BusinessObject> result = null;

            //instance.State = BusinessObject.BusinessObjectState.SearchObject;
            //var instance = search.BusinessObject;

            if (searchObject is Contact)
                result = ((Contact)searchObject).Search();

            data.SetContent(result.ToXmlString());
        }

        protected virtual void Save(Interface.Data data, Contact contact)
        {
            string errorMessage = string.Empty;
            //contact.Save();

        }

        protected virtual void Delete(Interface.Data data, XmlNode deleteNode)
        {

        }*/

        /*protected void Serialize(XmlWriter writer, BusinessObject businessObject)
        {
            var xs = new XmlSerializer(businessObject.GetType());
            xs.Serialize(writer, businessObject);
        }*/

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
    }
}
