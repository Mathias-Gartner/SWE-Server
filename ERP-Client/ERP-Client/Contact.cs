using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Client
{
    public class Contact
    {
        public string State { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public Address InvoiceAddress { get; set; }
        public Address DeliveryAddress { get; set; }
        public Contact BelongsTo { get; set; }
    }
}
