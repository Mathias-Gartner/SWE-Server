using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Database
{
    public class ContactDal : IBusinessObjectDal<Contact>
    {
        public string TableName
        {
            get { return "contacts"; }
        }

        public IEnumerable<string> Columns
        {
            get { return new[] { "id", "lastname", "firstname", "dateOfBirth", "email", "name", "uid", "prefix", "suffix" }; }
        }

        public Dictionary<string, object> CreateArguments(Contact instance)
        {
            var arguments = new Dictionary<string, object>();

            if (instance.ID >= 0)
                arguments.Add("id", instance.ID);
            if (!String.IsNullOrEmpty(instance.Lastname))
                arguments.Add("lastname", instance.Lastname);
            if (!String.IsNullOrEmpty(instance.Firstname))
                arguments.Add("firstname", instance.Firstname);
            if (instance.DateOfBirth != null)
                arguments.Add("dateOfBirth", instance.DateOfBirth);
            if (!String.IsNullOrEmpty(instance.Email))
                arguments.Add("email", instance.Email);
            if (!String.IsNullOrEmpty(instance.Name))
                arguments.Add("name", instance.Name);
            if (!String.IsNullOrEmpty(instance.Uid))
                arguments.Add("uid", instance.Uid);
            if (!String.IsNullOrEmpty(instance.Prefix))
                arguments.Add("prefix", instance.Prefix);
            if (!String.IsNullOrEmpty(instance.Suffix))
                arguments.Add("suffix", instance.Suffix);

            return arguments;
        }

        public ICollection<Contact> CreateObjectsFromSqlReader(SqlDataReader reader)
        {
            var contacts = new List<Contact>();
            while (reader.Read())
            {
                var contact = new Contact();
                contact.State = BusinessObject.BusinessObjectState.Unmodified;

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

                contacts.Add(contact);
            }
            return contacts;
        }
    }
}
