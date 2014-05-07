using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Definitions
{
    public class AddressDefinition : IDefinition
    {
        public string TableName
        {
            get { return "Address"; }
        }

        public IEnumerable<string> Columns
        {
            get { return new[] { "id", "name", "street", "number", "postOfficeBox", "postalCode", "city", "country" }; }
        }
        
        public Dictionary<string, object> CreateArguments(BusinessObject instance)
        {
            var address = (Address)instance;
            var arguments = new Dictionary<string, object>();

            if (address.ID >= 0)
                arguments.Add("id", address.ID);
            if (!String.IsNullOrEmpty(address.Name))
                arguments.Add("name", address.Name);
            if (!String.IsNullOrEmpty(address.Street))
                arguments.Add("street", address.Street);
            if (!String.IsNullOrEmpty(address.Number))
                arguments.Add("number", address.Number);
            if (!String.IsNullOrEmpty(address.PostOfficeBox))
                arguments.Add("postOfficeBox", address.PostOfficeBox);
            if (!String.IsNullOrEmpty(address.PostalCode))
                arguments.Add("postalCode", address.PostalCode);
            if (!String.IsNullOrEmpty(address.City))
                arguments.Add("city", address.City);
            if (!String.IsNullOrEmpty(address.Country))
                arguments.Add("country", address.Country);

            return arguments;
        }
        
        public ICollection<BusinessObject> CreateBusinessObjectsFromSqlReader(SqlDataReader reader, RelationLoader relationLoader)
        {
            var users = new List<BusinessObject>();
            while (reader.Read())
            {
                var address = new Address();
                address.State = BusinessObject.BusinessObjectState.Unmodified;

                address.ID = reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    address.Name = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    address.Street = reader.GetString(2);
                if (!reader.IsDBNull(3))
                    address.Number = reader.GetString(3);
                if (!reader.IsDBNull(4))
                    address.PostOfficeBox = reader.GetString(4);
                if (!reader.IsDBNull(5))
                    address.PostalCode = reader.GetString(5);
                if (!reader.IsDBNull(6))
                    address.City = reader.GetString(6);
                if (!reader.IsDBNull(7))
                    address.Country = reader.GetString(7);

                users.Add(address);
            }
            return users;
        }

        public bool SaveRelations(BusinessObject instance, RelationSaver relationSaver)
        {
            return true;
        }
    }
}
