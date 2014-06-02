using ErpPlugin.Data;
using ErpPlugin.Data.Database;
using ErpPlugin.Xml;
using Interface;
using log4net;
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
        ILog logger;

        public ErpPlugin()
        {
            logger = LogManager.GetLogger(GetType());
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
                logger.Warn("Erp request contains no postdata. Aborting.");
                return PutErrorMessage(data, "<h1>Postdata required</h1>");
            }

            logger.DebugFormat("Url Parameter req={0}", request.Url.Parameters["req"]);

            object result = null;
            try
            {
                switch (request.Url.Parameters["req"])
                {
                    case "searchContact":
                        var contact = request.RawPostData.FromXmlString<Contact>();
                        result = contact.Search();
                        if (logger.IsDebugEnabled)
                        {
                            var list = (IEnumerable<Contact>)result;
                            logger.DebugFormat("Contacts searched, {0} results", list.Count());
                        }
                        break;
                    case "saveContact":
                        contact = request.RawPostData.FromXmlString<Contact>();
                        result = contact.Save();
                        logger.DebugFormat("Contact saved, success={0}", result);
                        break;
                    case "deleteContact":
                        contact = request.RawPostData.FromXmlString<Contact>();
                        //result = contact.Delete();
                        logger.DebugFormat("Contact deleted, success={0}", result);
                        break;
                    case "searchInvoice":
                        var invoice = request.RawPostData.FromXmlString<Invoice>();
                        result = invoice.Search();
                        if (logger.IsDebugEnabled)
                        {
                            var list = (IEnumerable<Invoice>)result;
                            logger.DebugFormat("Invoices searched, {0} results", list.Count());
                        }
                        break;
                    case "saveInvoice":
                        invoice = request.RawPostData.FromXmlString<Invoice>();
                        result = invoice.Save();
                        logger.DebugFormat("Invoice saved, success={0}", result);
                        break;
                    case "generateInvoiceDocument":
                        invoice = request.RawPostData.FromXmlString<Invoice>();
                        var invoices = (IEnumerable<Invoice>)invoice.Search();
                        data.Content = invoices.Single().GenerateInvoiceDocument();
                        data.Contenttype = "application/pdf";
                        return data;
                }
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Contains("error in XML"))
                {
                    logger.Error("Xml validation error", e);
                    return PutErrorMessage(data, "<h1>Error in XML</h1>");
                }
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
