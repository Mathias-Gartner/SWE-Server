using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ERPClient
{
    public class InvoiceEntry
    {
        public string ID { get; set; }

        public string State { get; set; }

        public string Description { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }

        public int UStPercent { get; set; }

        public Invoice Invoice { get; set; }

        [XmlIgnore]
        public decimal NetPrice { get { return Price / (100 + UStPercent) * 100; } }
    }
}
