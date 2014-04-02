using ErpPlugin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin
{
    public class Contact : BusinessObject
    {
        public Contact()
        {
        }

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

        public ICollection<Contact> Search()
        {
            if (State != BusinessObjectState.SearchObject)
                throw new InvalidOperationException("Only SearchObject can be used for searching");

            try
            {
                return CurrentDalFactory.Instance.CreateDal().SearchContacts(this);
            }
            catch (ObjectNotFoundException)
            {
                return new List<Contact>();
            }
        }

        public static Contact CreateSearchObject()
        {
            return CreateSearchObject<Contact>();
        }

        public static Contact LoadObject(int id)
        {
            var dal = CurrentDalFactory.Instance.CreateDal();
            var contact = Contact.CreateSearchObject();
            contact.ID = id;
            return dal.SearchContacts(contact).SingleOrDefault();
        }
    }
}
