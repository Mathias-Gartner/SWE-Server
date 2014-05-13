using ErpPlugin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin
{
    public class Invoice : BusinessObject
    {
        public bool Outgoing { get; set; }

        public int InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Message { get; set; }

        public string Comment { get; set; }

        public Contact Contact { get; set; }

        public IList<InvoiceEntry> Entries { get; set; }

        public ICollection<Invoice> Search()
        {
            if (State != BusinessObjectState.SearchObject)
                throw new InvalidOperationException("Only SearchObjects can be used for searching");

            try
            {
                return CurrentDalFactory.Instance.CreateDal().SearchInvoice(this);
            }
            catch (ObjectNotFoundException)
            {
                return new List<Invoice>();
            }
        }

        public bool Save()
        {
            return CurrentDalFactory.Instance.CreateDal().SaveInvoice(this);
        }

        public static Invoice CreateSearchObject()
        {
            return CreateSearchObject<Invoice>();
        }
    }
}
