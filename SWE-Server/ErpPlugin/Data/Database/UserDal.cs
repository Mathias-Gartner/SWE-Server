using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Database
{
    public class UserDal : IBusinessObjectDal<User>
    {
        public string TableName
        {
            get { return "users"; }
        }

        public IEnumerable<string> Columns
        {
            get { return new[] { "id", "username", "password", "passwordSalt" }; }
        }

        public Dictionary<string, object> CreateArguments(User instance)
        {
            var arguments = new Dictionary<string, object>();

            if (instance.ID >= 0)
                arguments.Add("id", instance.ID);
            if (!String.IsNullOrEmpty(instance.Username))
                arguments.Add("lastname", instance.Username);
            if (!String.IsNullOrEmpty(instance.PasswordHash))
                arguments.Add("password", instance.PasswordHash);
            if (!String.IsNullOrEmpty(instance.PasswordSalt))
                arguments.Add("passwordSalt", instance.PasswordSalt);

            return arguments;
        }

        public ICollection<User> CreateObjectsFromSqlReader(System.Data.SqlClient.SqlDataReader reader)
        {
            var users = new List<User>();
            while (reader.Read())
            {
                var user = new User();
                user.ID = reader.GetInt32(0);
                user.Username = reader.GetString(1);
                user.PasswordHash = reader.GetString(2);
                user.PasswordSalt = reader.GetString(3);
                users.Add(user);
            }
            return users;
        }
    }
}
