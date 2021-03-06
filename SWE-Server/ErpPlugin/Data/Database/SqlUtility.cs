﻿using ErpPlugin.Data.Definitions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ErpPlugin.Data.Database
{
    public static class SqlUtility
    {
        static ILog logger = LogManager.GetLogger(typeof(SqlUtility));

        public static SqlDataReader SearchObjects(BusinessObject searchObject, IDefinition definition, Dictionary<string, object> arguments)
        {
            var sb = PrepareSelect(definition);
            AppendLikeWhereClause(sb, arguments);
            var queryString = sb.ToString();

            var queryManipulationingDefinition = definition as IQueryManipulatingDefinition;
            if (queryManipulationingDefinition != null)
            {
                queryString = queryManipulationingDefinition.FinalizeSearchQuery(searchObject, queryString, arguments);
            }

            using (var query = CreateQuery(queryString, ExtractParameters(arguments)))
            {
                return query.ExecuteReader();
            }
        }

        public static SqlDataReader LoadObjects(IDefinition definition, Dictionary<string, object> arguments)
        {
            var sb = PrepareSelect(definition);
            AppendWhereClause(sb, arguments);
            using (var query = CreateQuery(sb.ToString(), ExtractParameters(arguments)))
            {
                return query.ExecuteReader();
            }
        }

        public static object LoadRelatedObject(IDefinition definition, int id)
        {
            var arguments = new Dictionary<string, object>();
            arguments.Add("id", id);
            using (var reader = LoadObjects(definition, arguments))
            {
                return definition.CreateBusinessObjectsFromSqlReader(reader, LoadRelatedObject).Single();
            }
        }

        public static bool SaveObject(IDefinition definition, BusinessObject instance)
        {
            if (definition == null)
                throw new ArgumentNullException("IDefinition cannot be null");

            if (instance == null)
                return true; // true because everything that can be saved has been saved

            if (!definition.SaveRelations(instance, SaveObject))
                return false;

            string query;
            var arguments = definition.CreateArguments(instance);
            if (arguments.Count == 0 || arguments.Values.All(v => v == null))
                return true; // true because everything that can be saved has been saved

            if (instance.State == BusinessObjectState.New)
            {
                query = SqlUtility.PrepareInsert(definition, arguments).ToString();
            }
            else if (instance.State == BusinessObjectState.Modified)
            {
                var idArguments = new Dictionary<string, object>();
                idArguments.Add("id", instance.ID);
                var updateArguments = new Dictionary<string, object>();
                foreach (var argument in arguments)
                {
                    if (argument.Key != "id")
                        updateArguments.Add(argument.Key, argument.Value);
                }

                var sb = SqlUtility.PrepareUpdate(definition, updateArguments);
                SqlUtility.AppendWhereClause(sb, idArguments);
                query = sb.ToString();
            }
            else if (instance.State == BusinessObjectState.Unmodified)
                return true;
            else
            {
                throw new InvalidOperationException(
                    String.Format("BusinessObject is in illegal State {0}", instance.State.ToString()));
            }

            using (var command = SqlUtility.CreateQuery(query, SqlUtility.ExtractParameters(arguments)))
            {
                var id = command.ExecuteScalar();
                if (id is DBNull)
                    return false;

                instance.ID = (int)id;
                instance.State = BusinessObjectState.Unmodified;
            }

            return true;
        }

        public static SqlCommand CreateQuery(string queryString, IEnumerable<SqlParameter> parameters)
        {
            logger.DebugFormat("Query created: {0}", queryString);
            var query = new SqlCommand(queryString, GetConnection());
            if (parameters != null)
                query.Parameters.AddRange(parameters.ToArray());

            return query;
        }

        public static SqlConnection GetConnection()
        {
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ErpConnectionString"].ConnectionString);
            con.Open();
            return con;
        }

        public static IEnumerable<SqlParameter> ExtractParameters(Dictionary<string, object> arguments)
        {
            return arguments.Keys.Select(key => new SqlParameter(String.Format("@{0}", key), arguments[key] == null ? DBNull.Value : arguments[key]));
        }

        public static void AppendWhereClause(StringBuilder sb, Dictionary<string, object> arguments)
        {
            if (sb == null)
                throw new ArgumentNullException("StringBuilder cannot be null");

            if (arguments != null && arguments.Keys.Count > 0)
                sb.AppendFormat(" WHERE {0}", String.Join(" and ", arguments.Keys.Select(key => String.Format("{0}=@{0}", key))));
        }

        public static void AppendLikeWhereClause(StringBuilder sb, Dictionary<string, object> arguments)
        {
            if (sb == null)
                throw new ArgumentNullException("StringBuilder cannot be null");

            if (arguments != null && arguments.Keys.Count > 0)
                sb.AppendFormat(" WHERE {0}", String.Join(" and ", arguments.Keys.Select(key => String.Format("{0} like @{0}+'%'", key))));
        }

        public static StringBuilder PrepareSelect(IDefinition definition)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("SELECT {0} FROM {1}", String.Join(", ", definition.Columns.Select(c => definition.TableName + "." + c)), definition.TableName);
            return sb;
        }

        public static StringBuilder PrepareInsert(IDefinition definition, Dictionary<string, object> arguments)
        {
            if (definition == null)
                throw new ArgumentNullException("IDefinition cannot be null");

            if (arguments == null || arguments.Keys.Count < 1)
                throw new InvalidOperationException("Cannot build valid statement without arguments");

            var sb = new StringBuilder();
            sb = sb.AppendFormat("INSERT INTO {0}({1}) OUTPUT INSERTED.id VALUES ({2})",
                                    definition.TableName,
                                    String.Join(", ", arguments.Keys.ToArray()),
                                    String.Join(", ", arguments.Keys.Select(key => String.Format("@{0}", key))));
            return sb;
        }

        public static StringBuilder PrepareUpdate(IDefinition definition, Dictionary<string, object> arguments)
        {
            if (definition == null)
                throw new ArgumentNullException("IDefinition cannot be null");

            if (arguments == null || arguments.Keys.Count < 1)
                throw new InvalidOperationException("Cannot build valid statement without arguments");

            var sb = new StringBuilder();
            sb.AppendFormat("UPDATE {0} SET ", definition.TableName);
            sb.Append(String.Join(", ", arguments.Keys.Select(key => String.Format("{0}=@{0}", key))));
            sb.Append(" OUTPUT INSERTED.id");
            return sb;
        }

        public static StringBuilder PrepareDelete(IDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException("IDefinition cannot be null");

            var sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(definition.TableName);
            return sb;
        }
    }
}
