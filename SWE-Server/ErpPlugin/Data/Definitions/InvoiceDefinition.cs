using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Definitions
{
    public class InvoiceDefinition : IDefinition, IQueryManipulatingDefinition
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
            if (invoice.Outgoing.HasValue || instance.State != BusinessObject.BusinessObjectState.SearchObject)
                arguments.Add("outgoingInvoice", invoice.Outgoing);
            if (invoice.InvoiceNumber >= 0 || invoice.State != BusinessObject.BusinessObjectState.SearchObject)
                arguments.Add("invoiceNumber", invoice.InvoiceNumber);
            if (invoice.InvoiceDate.HasValue || instance.State != BusinessObject.BusinessObjectState.SearchObject)
                arguments.Add("invoiceDate", invoice.InvoiceDate);
            if (invoice.DueDate.HasValue || instance.State != BusinessObject.BusinessObjectState.SearchObject)
                arguments.Add("dueDate", invoice.DueDate);
            if (!String.IsNullOrEmpty(invoice.Message) || instance.State != BusinessObject.BusinessObjectState.SearchObject)
                arguments.Add("message", invoice.Message);
            if (!String.IsNullOrEmpty(invoice.Comment) || instance.State != BusinessObject.BusinessObjectState.SearchObject)
                arguments.Add("comment", invoice.Comment);

            // relations
            if (invoice.Contact != null && invoice.Contact.ID >= 0)
                arguments.Add("contactId", invoice.Contact.ID);
            else if (instance.State != BusinessObject.BusinessObjectState.SearchObject)
                arguments.Add("contactId", null);

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

        public string FinalizeSearchQuery(BusinessObject instance, string query, IDictionary<string, object> arguments)
        {
            var invoice = (Invoice)instance;
            var sb = new StringBuilder(query);

            if (invoice.InvoiceDateFrom.HasValue)
            {
                AddWhereCondition(sb, "invoiceDate >= %invoiceDateFrom%");
                arguments.Add("invoiceDateFrom", invoice.InvoiceDateFrom);
            }
            if (invoice.InvoiceDateTo.HasValue)
            {
                AddWhereCondition(sb, "invoiceDate <= %invoiceDateTo%");
                arguments.Add("invoiceDateTo", invoice.InvoiceDateTo);
            }

            var newQuery = sb.ToString();

            if (invoice.InvoiceNumber < 1 && query.Contains("%invoiceNumber%"))
            {
                newQuery = newQuery.Replace("%invoiceNumber%", "(SELECT MAX(InvoiceNumber)+1 FROM invoices)");
            }
            
            if (invoice.SumFrom.HasValue || invoice.SumTo.HasValue)
            {
                var index = newQuery.IndexOf(" WHERE");
                if (index < 0) index = newQuery.Length;
                newQuery = newQuery.Insert(index, "JOIN invoiceEntries e ON e.invoiceId = invoice.id")
                    + " GROUP BY " + newQuery.Substring(7, newQuery.IndexOf(" FROM") - 7)
                    + " HAVING ";

                if (invoice.SumFrom.HasValue)
                {
                    newQuery = newQuery + "sum(e.price * e.amount) >= %sumFrom% and ";
                    arguments.Add("sumFrom", invoice.SumFrom);
                }
                if (invoice.SumTo.HasValue)
                {
                    newQuery = newQuery + "sum(e.price * e.amount) <= %sumTo% and ";
                    arguments.Add("sumTo", invoice.SumTo);
                }

                newQuery = newQuery.Substring(0, newQuery.Length - 5);
            }

            return newQuery;
        }

        private void AddWhereCondition(StringBuilder sb, string condition)
        {
            if (sb.ToString().Contains("WHERE"))
                sb.Append(" and ");
            else
                sb.Append(" WHERE ");

            sb.Append(condition);
        }
    }
}
