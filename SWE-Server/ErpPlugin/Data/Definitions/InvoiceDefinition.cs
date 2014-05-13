using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Definitions
{
    public class InvoiceDefinition : IDefinition
    {
        public string TableName
        {
            get { return "invoices"; }
        }

        public IEnumerable<string> Columns
        {
            get { return new[] { "id", "outgoingInvoice", "invoiceNumber", "invoiceDate", "dueDate", "message", "comment", "contactId" }; }
        }

        public Dictionary<string, object> CreateArguments(BusinessObject instance)
        {
            var invoice = (Invoice)instance;
            var arguments = new Dictionary<string, object>();

            if (invoice.ID >= 0)
                arguments.Add("id", invoice.ID);
            arguments.Add("outgoingInvoice", invoice.Outgoing);
            if (invoice.InvoiceNumber >= 0)
                arguments.Add("invoiceNumber", invoice.InvoiceNumber);
            arguments.Add("invoiceDate", invoice.InvoiceDate);
            if (invoice.DueDate.HasValue)
                arguments.Add("dueDate", invoice.DueDate);
            if (!String.IsNullOrEmpty(invoice.Message))
                arguments.Add("message", invoice.Message);
            if (!String.IsNullOrEmpty(invoice.Comment))
                arguments.Add("comment", invoice.Comment);

            // relations
            if (invoice.Contact != null && invoice.Contact.ID >= 0)
                arguments.Add("contactId", invoice.Contact.ID);

            return arguments;
        }

        public ICollection<BusinessObject> CreateBusinessObjectsFromSqlReader(SqlDataReader reader, RelationLoader relationLoader)
        {
            var invoices = new List<BusinessObject>();
            while (reader.Read())
            {
                var invoice = new Invoice();
                invoice.State = BusinessObject.BusinessObjectState.Unmodified;

                invoice.ID = reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    invoice.Outgoing = reader.GetBoolean(1);
                if (!reader.IsDBNull(2))
                    invoice.InvoiceNumber = reader.GetInt32(2);
                if (!reader.IsDBNull(3))
                    invoice.InvoiceDate = reader.GetDateTime(3);
                if (!reader.IsDBNull(4))
                    invoice.DueDate = reader.GetDateTime(4);
                if (!reader.IsDBNull(5))
                    invoice.Message = reader.GetString(5);
                if (!reader.IsDBNull(6))
                    invoice.Comment = reader.GetString(6);
                if (!reader.IsDBNull(7))
                    invoice.Contact = (Contact)relationLoader(new ContactDefinition(), reader.GetInt32(7));

                invoices.Add(invoice);
            }
            return invoices;
        }

        public bool SaveRelations(BusinessObject instance, RelationSaver relationSaver)
        {
            var invoice = (Invoice)instance;
            
            foreach (var entry in invoice.Entries)
            {
                if (!relationSaver(new InvoiceEntryDefinition(), entry))
                    return false;
            }

            return relationSaver(new ContactDefinition(), invoice.Contact);
        }
    }
}
