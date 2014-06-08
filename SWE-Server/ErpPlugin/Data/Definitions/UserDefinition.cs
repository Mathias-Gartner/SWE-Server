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
    public class UserDefinition : IDefinition
    {
        public string TableName
        {
            get { return "users"; }
        }

        public IEnumerable<string> Columns
        {
            get { return new[] { "id", "username", "password", "passwordSalt" }; }
        }

        public Dictionary<string, object> CreateArguments(BusinessObject instance)
        {
            var user = (User)instance;
            var arguments = new Dictionary<string, object>();

            if (user.ID >= 0)
                arguments.Add("id", user.ID);
            if (!String.IsNullOrEmpty(user.Username))
                arguments.Add("lastname", user.Username);
            if (!String.IsNullOrEmpty(user.PasswordHash))
                arguments.Add("password", user.PasswordHash);
            if (!String.IsNullOrEmpty(user.PasswordSalt))
                arguments.Add("passwordSalt", user.PasswordSalt);

            return arguments;
        }

        public ICollection<BusinessObject> CreateBusinessObjectsFromSqlReader(DbDataReader reader, RelationLoader relationLoader)
        {
            var users = new List<BusinessObject>();
            while (reader.Read())
            {
                var user = new User();
                user.State = BusinessObjectState.Unmodified;

                user.ID = reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    user.Username = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    user.PasswordHash = reader.GetString(2);
                if (!reader.IsDBNull(3))
                    user.PasswordSalt = reader.GetString(3);

                users.Add(user);
            }
            return users;
        }

        public bool SaveRelations(BusinessObject instance, RelationSaver relationSaver)
        {
            return true;
        }
    }
}
