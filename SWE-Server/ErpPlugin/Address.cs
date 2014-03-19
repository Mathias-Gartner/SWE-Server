using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin
{
    public class Address : BusinessObject
    {
        public string Name { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string PostOfficeBox { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }
}
