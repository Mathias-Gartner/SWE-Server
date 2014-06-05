using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Client
{
    public class Invoice
    {
        public bool Outgoing { get; set; }

        public int InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Message { get; set; }

        public string Comment { get; set; }

        public Contact Contact { get; set; }

        public List<InvoiceEntry> Entries { get; set; }
    }
}
