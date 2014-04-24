using ErpPlugin.Data.Definitions;
using log4net;
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
        ILog logger;
        IDefinitionFactory _factory;

        public Dal(IDefinitionFactory definitionFactory)
        {
            logger = LogManager.GetLogger(GetType());
            _factory = definitionFactory;
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

            var definition = _factory.CreateDefinitionForType<T>();
            var arguments = definition.CreateArguments(searchObject);

            ICollection<T> objects = null;
            using (var reader = SqlUtility.LoadObjects(definition, arguments))
            {
                if (!reader.HasRows)
                    throw new ObjectNotFoundException();

                objects = definition.CreateBusinessObjectsFromSqlReader(reader, SqlUtility.LoadRelatedObject).Cast<T>().ToList();
            }
            return objects;
        }

        public bool SaveBusinessObject<T>(T instance) where T : BusinessObject
        {
            var definition = _factory.CreateDefinitionForType<T>();

            return SqlUtility.SaveObject(definition, instance);
        }

        public bool DeleteBusinessObject<T>(T instance) where T : BusinessObject
        {
            var definition = _factory.CreateDefinitionForType<T>();
            var arguments = new Dictionary<string, object>();
            arguments.Add("id", instance.ID);
            var sb = SqlUtility.PrepareDelete(definition);
            SqlUtility.AppendWhereClause(sb, arguments);
            return SqlUtility.CreateQuery(sb.ToString(), SqlUtility.ExtractParameters(arguments)).ExecuteNonQuery() == 1;
        }


    }
}
