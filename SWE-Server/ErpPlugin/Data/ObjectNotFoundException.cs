using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() : base("The requested Object cannot be found.")
        {
        }

        public ObjectNotFoundException(string message) : base(message)
        {
        }
    }
}
