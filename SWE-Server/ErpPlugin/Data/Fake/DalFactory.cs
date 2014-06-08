using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Fake
{
    public class DalFactory : IDalFactory
    {
        public IDal CreateDal()
        {
            return new Dal();
        }
    }
}
