using System;

namespace ERP_Client
{
    public class Contact
    {
        public string ID { get; set; }
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

        public override string ToString()
        {
            if (String.IsNullOrEmpty(Name))
                return String.Join(" ", new[] { Firstname, Lastname });
            return Name;
        }
    }
}
