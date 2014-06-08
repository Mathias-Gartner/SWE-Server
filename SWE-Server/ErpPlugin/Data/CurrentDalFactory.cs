using ErpPlugin.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data
{
    public static class CurrentDalFactory
    {
        private static IDalFactory _instance;

        public static IDalFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DalFactory();

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}
