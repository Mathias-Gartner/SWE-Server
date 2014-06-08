using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Definitions
{
    public class ContactDefinition : IDefinition
    {
        public string TableName
        {
            get { return "contacts"; }
        }

        public IEnumerable<string> Columns
        {
            get { return new[] { "id", "lastname", "firstname", "dateOfBirth", "email", "name", "uid", "prefix", "suffix", "addressId", "deliveryAddressId", "invoiceAddressId", "belongsToId" }; }
        }

        public Dictionary<string, object> CreateArguments(BusinessObject instance)
        {
            var contact = (Contact)instance;
            var arguments = new Dictionary<string, object>();

            if (contact.ID >= 0)
                arguments.Add("id", contact.ID);
            if (!String.IsNullOrEmpty(contact.Lastname) || instance.State != BusinessObjectState.SearchObject)
                arguments.Add("lastname", contact.Lastname);
            if (!String.IsNullOrEmpty(contact.Firstname) || instance.State != BusinessObjectState.SearchObject)
                arguments.Add("firstname", contact.Firstname);
            if (contact.DateOfBirth != null || instance.State != BusinessObjectState.SearchObject)
                arguments.Add("dateOfBirth", contact.DateOfBirth);
            if (!String.IsNullOrEmpty(contact.Email) || instance.State != BusinessObjectState.SearchObject)
                arguments.Add("email", contact.Email);
            if (!String.IsNullOrEmpty(contact.Name) || instance.State != BusinessObjectState.SearchObject)
                arguments.Add("name", contact.Name);
            if (!String.IsNullOrEmpty(contact.Uid) || instance.State != BusinessObjectState.SearchObject)
                arguments.Add("uid", contact.Uid);
            if (!String.IsNullOrEmpty(contact.Prefix) || instance.State != BusinessObjectState.SearchObject)
                arguments.Add("prefix", contact.Prefix);
            if (!String.IsNullOrEmpty(contact.Suffix) || instance.State != BusinessObjectState.SearchObject)
                arguments.Add("suffix", contact.Suffix);

            // relations
            if (contact.Address != null && contact.Address.ID >= 0)
                arguments.Add("addressId", contact.Address.ID);
            else if (instance.State != BusinessObjectState.SearchObject)
                arguments.Add("addressId", null);
            if (contact.DeliveryAddress != null && contact.DeliveryAddress.ID >= 0)
                arguments.Add("deliveryAddressId", contact.DeliveryAddress.ID);
            else if (instance.State != BusinessObjectState.SearchObject)
                arguments.Add("deliveryAddressId", null);
            if (contact.InvoiceAddress != null && contact.InvoiceAddress.ID >= 0)
                arguments.Add("invoiceAddressId", contact.InvoiceAddress.ID);
            else if (instance.State != BusinessObjectState.SearchObject)
                arguments.Add("invoiceAddressId", null);
            if (contact.BelongsTo != null && contact.BelongsTo.ID >= 0)
                arguments.Add("belongsToId", contact.BelongsTo.ID);
            else if (instance.State != BusinessObjectState.SearchObject)
                arguments.Add("belongsToId", null);

            return arguments;
        }

        public ICollection<BusinessObject> CreateBusinessObjectsFromSqlReader(DbDataReader reader, RelationLoader relationLoader)
        {
            var contacts = new List<BusinessObject>();
            while (reader.Read())
            {
                var contact = new Contact();
                contact.State = BusinessObjectState.Unmodified;

                contact.ID = reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    contact.Lastname = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    contact.Firstname = reader.GetString(2);
                if (!reader.IsDBNull(3))
                    contact.DateOfBirth = reader.GetDateTime(3);
                if (!reader.IsDBNull(4))
                    contact.Email = reader.GetString(4);
                if (!reader.IsDBNull(5))
                    contact.Name = reader.GetString(5);
                if (!reader.IsDBNull(6))
                    contact.Uid = reader.GetString(6);
                if (!reader.IsDBNull(7))
                    contact.Prefix = reader.GetString(7);
                if (!reader.IsDBNull(8))
                    contact.Suffix = reader.GetString(8);
                if (!reader.IsDBNull(9))
                    contact.Address = (Address)relationLoader(new AddressDefinition(), reader.GetInt32(9));
                if (!reader.IsDBNull(10))
                    contact.DeliveryAddress = (Address)relationLoader(new AddressDefinition(), reader.GetInt32(10));
                if (!reader.IsDBNull(11))
                    contact.InvoiceAddress = (Address)relationLoader(new AddressDefinition(), reader.GetInt32(11));
                if (!reader.IsDBNull(12))
                    contact.BelongsTo = (Contact)relationLoader(new ContactDefinition(), reader.GetInt32(12));

                contacts.Add(contact);
            }
            return contacts;
        }

        public bool SaveRelations(BusinessObject instance, RelationSaver relationSaver)
        {
            var contact = (Contact)instance;

            if (!relationSaver(new AddressDefinition(), contact.Address))
                return false;
            if (!relationSaver(new AddressDefinition(), contact.DeliveryAddress))
                return false;
            if (!relationSaver(new AddressDefinition(), contact.InvoiceAddress))
                return false;
            if (!relationSaver(new ContactDefinition(), contact.BelongsTo))
                return false;

            return true;
        }
    }
}
