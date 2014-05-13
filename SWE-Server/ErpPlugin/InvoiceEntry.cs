using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin
{
    public class InvoiceEntry : BusinessObject
    {
        public string Description { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }

        public int UStPercent { get; set; }

        public Invoice Invoice { get; set; }
    }
}
