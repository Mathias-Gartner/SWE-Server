using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Definitions
{
    public interface IQueryManipulatingDefinition
    {
        string FinalizeSearchQuery(BusinessObject instance, string query, IDictionary<string, object> arguments);
    }
}
