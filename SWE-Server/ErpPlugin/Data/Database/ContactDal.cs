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
            if (!String.IsNullOrEmpty("uid"))
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
                contact.ID = reader.GetInt32(0);
                contact.Lastname = reader.GetString(1);
                contact.Firstname = reader.GetString(2);
                contact.DateOfBirth = reader.GetDateTime(3);
                contact.Email = reader.GetString(4);
                contact.Name = reader.GetString(5);
                contact.Uid = reader.GetString(6);
                contact.Prefix = reader.GetString(7);
                contact.Suffix = reader.GetString(8);
                contacts.Add(contact);
            }
            return contacts;
        }
    }
}
