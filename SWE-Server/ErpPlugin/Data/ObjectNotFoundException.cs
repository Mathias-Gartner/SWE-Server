using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data
{
    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException()
            : base("The requested Object cannot be found.")
        {
        }

        public ObjectNotFoundException(string message)
            : base(message)
        {
        }

        public ObjectNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
