using System.Collections.Generic;
using System.Data.SqlClient;

namespace ErpPlugin.Data.Database
{
    public interface IBusinessObjectDal<T> : IBusinessObjectDal
    {
        Dictionary<string,object> CreateArguments(T instance);

        ICollection<T> CreateObjectsFromSqlReader(SqlDataReader reader);
    }

    public interface IBusinessObjectDal
    {
        string TableName { get; }

        IEnumerable<string> Columns { get; }
    }
}
