using ErpPlugin.Data.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Database
{
    public class DatabaseDalFactory : IDalFactory
    {
        public IDal CreateDal()
        {
            return new Dal(new DefinitionFactory());
        }
    }
}
