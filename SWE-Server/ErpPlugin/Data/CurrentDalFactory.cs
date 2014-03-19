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
        private static IDalFactory _instance = null;

        public static IDalFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatabaseDalFactory();

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}
