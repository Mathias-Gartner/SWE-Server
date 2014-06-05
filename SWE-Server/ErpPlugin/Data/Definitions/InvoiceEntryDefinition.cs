using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Definitions
{
    public class InvoiceEntryDefinition : IDefinition
    {
        public string TableName
        {
            get { return "invoiceEntries"; }
        }

        public IEnumerable<string> Columns
        {
            get { return new[] { "id", "description", "amount", "price", "ustPercent", "invoiceId" }; }
        }

        public Dictionary<string, object> CreateArguments(BusinessObject instance)
        {
            var invoiceEntry = (InvoiceEntry)instance;
            var arguments = new Dictionary<string, object>();

            if (invoiceEntry.ID >= 0)
                arguments.Add("id", invoiceEntry.ID);
            if (!String.IsNullOrEmpty(invoiceEntry.Description) || instance.State != BusinessObject.BusinessObjectState.SearchObject)
                arguments.Add("description", invoiceEntry.Description);
            if (invoiceEntry.Amount >= 0)
                arguments.Add("amount", invoiceEntry.Amount);
            arguments.Add("price", invoiceEntry.Price);
            if (invoiceEntry.UStPercent >= 0)
                arguments.Add("ustPercent", invoiceEntry.UStPercent);

            //relations
            if (invoiceEntry.Invoice != null && invoiceEntry.Invoice.ID >= 0)
                arguments.Add("invoiceId", invoiceEntry.Invoice.ID);
            else if (instance.State != BusinessObject.BusinessObjectState.SearchObject)
                arguments.Add("invoiceId", null);

            return arguments;
        }

        public ICollection<BusinessObject> CreateBusinessObjectsFromSqlReader(SqlDataReader reader, RelationLoader relationLoader)
        {
            var invoiceEntries = new List<BusinessObject>();
            while (reader.Read())
            {
                var invoiceEntry = new InvoiceEntry();
                invoiceEntry.State = BusinessObject.BusinessObjectState.Unmodified;

                if (!reader.IsDBNull(0))
                    invoiceEntry.ID = reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    invoiceEntry.Description = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    invoiceEntry.Amount = reader.GetInt32(2);
                if (!reader.IsDBNull(3))
                    invoiceEntry.Price = reader.GetDecimal(3);
                if (!reader.IsDBNull(4))
                    invoiceEntry.UStPercent = reader.GetInt32(4);
                if (!reader.IsDBNull(5))
                    invoiceEntry.Invoice = (Invoice)relationLoader(new InvoiceDefinition(), reader.GetInt32(5));

                invoiceEntries.Add(invoiceEntry);
            }
            return invoiceEntries;
        }

        public bool SaveRelations(BusinessObject instance, RelationSaver relationSaver)
        {
            var invoiceEntry = (InvoiceEntry)instance;
            return relationSaver(new InvoiceDefinition(), invoiceEntry.Invoice);
        }
    }
}
