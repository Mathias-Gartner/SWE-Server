using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPClient
{
    public class Invoice
    {
        public Invoice()
        {
            this.InvoiceNumber = -1;
        }

        public string ID { get; set; }

        public string State { get; set; }

        public bool? Outgoing { get; set; }

        public int InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDateFrom { get; set; }

        public DateTime? InvoiceDateTo { get; set; }

        public DateTime? DueDate { get; set; }

        public string Message { get; set; }

        public string Comment { get; set; }

        public Contact Contact { get; set; }

        public Collection<InvoiceEntry> Entries { get; set; }

        public decimal Sum { get { return Entries == null ? 0 : Entries.Sum(e => e.Amount * e.Price); } }

        public decimal? SumFrom { get; set; }

        public decimal? SumTo { get; set; }
    }
}
