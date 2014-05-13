using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ErpPlugin.Data.Definitions
{
    public delegate object RelationLoader(IDefinition definition, int id);
    public delegate bool RelationSaver(IDefinition definition, BusinessObject instance);

    public interface IDefinition
    {
        string TableName { get; }

        IEnumerable<string> Columns { get; }

        //    Column name, value
        Dictionary<string, object> CreateArguments(BusinessObject instance);

        ICollection<BusinessObject> CreateBusinessObjectsFromSqlReader(SqlDataReader reader, RelationLoader relationLoader);

        bool SaveRelations(BusinessObject instance, RelationSaver relationSaver);
    }
}
