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

        public ICollection<User> Search(User searchObject)
        {
            return SearchBusinessObject(searchObject);
        }

        public ICollection<Contact> Search(Contact searchObject)
        {
            return SearchBusinessObject(searchObject);
        }

        public bool Save(Contact contact)
        {
            return SaveBusinessObject(contact);
        }

        public bool Delete(Contact contact)
        {
            return DeleteBusinessObject(contact);
        }

        public ICollection<Invoice> SearchInvoice(Invoice searchObject)
        {
            return SearchBusinessObject(searchObject);
        }

        public bool Save(Invoice invoice)
        {
            return SaveBusinessObject(invoice);
        }

        public bool Delete(Invoice invoice)
        {
            return DeleteBusinessObject(invoice);
        }

        public ICollection<T> SearchBusinessObject<T>(T searchObject) where T : BusinessObject
        {
            if (searchObject == null || searchObject.State != BusinessObjectState.SearchObject)
            {
                throw new InvalidOperationException("searchObject is not a SearchObject");
            }

            var definition = _factory.CreateDefinitionForType<T>();
            var arguments = definition.CreateArguments(searchObject);

            ICollection<T> objects = null;
            using (var reader = SqlUtility.SearchObjects(searchObject, definition, arguments))
            {
                if (!reader.HasRows)
                    throw new ObjectNotFoundException();

                objects = definition.CreateBusinessObjectsFromSqlReader(reader, SqlUtility.LoadRelatedObject).Cast<T>().ToList();
            }
            return objects;
        }

        public bool SaveBusinessObject<T>(T businessObject) where T : BusinessObject
        {
            var definition = _factory.CreateDefinitionForType<T>();

            return SqlUtility.SaveObject(definition, businessObject);
        }

        public bool DeleteBusinessObject<T>(T businessObject) where T : BusinessObject
        {
            if (businessObject == null)
                return true;

            var definition = _factory.CreateDefinitionForType<T>();
            var arguments = new Dictionary<string, object>();
            arguments.Add("id", businessObject.ID);
            var sb = SqlUtility.PrepareDelete(definition);
            SqlUtility.AppendWhereClause(sb, arguments);
            using (var query = SqlUtility.CreateQuery(sb.ToString(), SqlUtility.ExtractParameters(arguments)))
            {
                return query.ExecuteNonQuery() == 1;
            }
        }
    }
}
