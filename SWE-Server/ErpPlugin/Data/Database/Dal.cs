using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Database
{
    public class Dal : IDal
    {
        IBusinessObjectDalFactory _factory;

        public Dal(IBusinessObjectDalFactory dalFactory)
        {
            _factory = dalFactory;
        }

        public ICollection<User> SearchUsers(User searchObject)
        {
            return SearchBusinessObject<User>(searchObject);
        }

        public ICollection<Contact> SearchContacts(Contact searchObject)
        {
            return SearchBusinessObject<Contact>(searchObject);
        }

        public bool SaveContact(Contact contact)
        {
            return SaveBusinessObject<Contact>(contact);
        }

        public bool DeleteContact(Contact contact)
        {
            return DeleteBusinessObject<Contact>(contact);
        }

        public ICollection<T> SearchBusinessObject<T>(T searchObject) where T : BusinessObject
        {
            if (searchObject.State != BusinessObject.BusinessObjectState.SearchObject)
            {
                throw new InvalidOperationException("searchObject is not a SearchObject");
            }

            var dal = _factory.CreateDalForType<T>();
            var arguments = dal.CreateArguments(searchObject);

            ICollection<T> objects = null;
            using (var reader = LoadObjects(dal, arguments))
            {
                if (!reader.HasRows)
                    throw new ObjectNotFoundException();

                objects = dal.CreateObjectsFromSqlReader(reader);
            }
            return objects;
        }

        /*protected SqlDataReader LoadObject(IBusinessObjectDal dal, int id)
        {
            var arguments = new Dictionary<string, object>();
            arguments.Add("id", instance.ID);
            var sb = PrepareSelect(dal);
            AppendWhereClause(sb, arguments);
            return ExecuteQuery(sb.ToString(), ExtractParameters(arguments));
        }*/

        protected SqlDataReader LoadObjects(IBusinessObjectDal dal, Dictionary<string, object> arguments)
        {
            var sb = PrepareSelect(dal);
            AppendWhereClause(sb, arguments);
            return ExecuteQuery(sb.ToString(), ExtractParameters(arguments));
        }

        public bool SaveBusinessObject<T>(T instance) where T : BusinessObject
        {
            string query;
            var dal = _factory.CreateDalForType<T>();
            var arguments = dal.CreateArguments(instance);

            if (instance.State == BusinessObject.BusinessObjectState.New)
            {
                query = string.Format("INSERT INTO {0}({1}) VALUES ({2})",
                                    dal.TableName,
                                    String.Join(", ", arguments.Keys.ToArray()),
                                    String.Join(", ", arguments.Keys.Select(key=>String.Format("@{0}", key))));
                // TODO: Set ID in BusinessObject
            }
            else if (instance.State == BusinessObject.BusinessObjectState.Modified)
            {
                var idArguments = new Dictionary<string, object>();
                idArguments.Add("id", instance.ID);
                var sb = PrepareUpdate(dal, arguments);
                AppendWhereClause(sb, idArguments);
                query = sb.ToString();
            }
            else
            {
                throw new InvalidOperationException(
                    String.Format("BusinessObject is in illegal State {0}", instance.State.ToString()));
            }

            return CreateQuery(query, ExtractParameters(arguments)).ExecuteNonQuery() == 1;
        }

        public bool DeleteBusinessObject<T>(T instance) where T : BusinessObject
        {
            var dal = _factory.CreateDalForType<T>();
            var arguments = new Dictionary<string, object>();
            arguments.Add("id", instance.ID);
            var sb = PrepareDelete(dal);
            AppendWhereClause(sb, arguments);
            return CreateQuery(sb.ToString(), ExtractParameters(arguments)).ExecuteNonQuery() == 1;
        }

        protected SqlCommand CreateQuery(string queryString, IEnumerable<SqlParameter> parameters)
        {
            var query = new SqlCommand(queryString, GetConnection());
            if (parameters != null)
                query.Parameters.AddRange(parameters.ToArray());

            return query;
        }

        protected SqlDataReader ExecuteQuery(string queryString, IEnumerable<SqlParameter> parameters = null)
        {
            var query = CreateQuery(queryString, parameters);
            return query.ExecuteReader();
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["ErpConnectionString"].ConnectionString);
        }

        public static IEnumerable<SqlParameter> ExtractParameters(Dictionary<string, object> arguments)
        {
            return arguments.Keys.Select(key => new SqlParameter(String.Format("@{0}", key), arguments[key]));
        }

        public static void AppendWhereClause(StringBuilder sb, Dictionary<string, object> arguments)
        {
            sb.AppendFormat(" WHERE {0}", String.Join(" and ", arguments.Keys.Select(key=>String.Format("{0}=@{0}", key))));
        }

        public static StringBuilder PrepareSelect(IBusinessObjectDal dal)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("SELECT {0} FROM {1}", String.Join(", ", dal.Columns), dal.TableName);
            return sb;
        }

        public static StringBuilder PrepareUpdate(IBusinessObjectDal dal, Dictionary<string, object> arguments)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("UPDATE {0} SET ", dal.TableName);
            sb.Append(String.Join(", ", arguments.Keys.Select(key => String.Format("{0}=@{0}", key))));
            return sb;
        }

        public static StringBuilder PrepareDelete(IBusinessObjectDal dal)
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(dal.TableName);
            return sb;
        }
    }
}
