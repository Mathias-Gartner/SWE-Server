using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Fake
{
    public class Dal : IDal
    {
        public static bool ContactSaved;
        public static bool InvoiceSaved;
        public static bool BusinessObjectSaved;

        public static void Reset()
        {
            ContactSaved = false;
            InvoiceSaved = false;
            BusinessObjectSaved = false;
        }

        IEnumerable<Contact> contacts = new[]
            {
                new Contact(){ID = 1, Lastname = "Huber", Firstname = "Hans", DateOfBirth = DateTime.Parse("1960-01-01")},
                new Contact(){ID = 2, Lastname = "Huber", Firstname = "Peter", DateOfBirth = DateTime.Parse("1950-01-01")}
            };

        public ICollection<User> Search(User searchObject)
        {
            if (searchObject == null)
                throw new ArgumentNullException("searchObject");

            if (searchObject.State == BusinessObjectState.SearchObject && searchObject.Username == "User1")
            {
                var salt = User.CreateSalt();
                return new[] { new User() { ID = 1, PasswordSalt = salt, PasswordHash = User.CreatePasswordHash("123456", salt), Username = "User1" } };
            }
            return new User[] { };
        }

        public ICollection<Contact> Search(Contact searchObject)
        {
            if (searchObject.State != BusinessObjectState.SearchObject)
            {
                throw new InvalidOperationException("searchObject is not a SearchObject");
            }

            if (!String.IsNullOrEmpty(searchObject.Lastname))
            {
                if (contacts.Any(c => c.Lastname == searchObject.Lastname))
                    return contacts.Where(c => c.Lastname == searchObject.Lastname).ToList();
            }

            throw new ObjectNotFoundException();
        }

        public bool Save(Contact contact)
        {
            ContactSaved = true;
            contact.State = BusinessObjectState.Unmodified;
            return true;
        }

        public bool Delete(Contact contact)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> SearchBusinessObject<T>(T searchObject) where T : BusinessObject
        {
            throw new NotImplementedException();
        }

        public bool SaveBusinessObject<T>(T businessObject) where T : BusinessObject
        {
            BusinessObjectSaved = true;
            return true;
        }

        public bool DeleteBusinessObject<T>(T businessObject) where T : BusinessObject
        {
            throw new NotImplementedException();
        }

        public ICollection<Invoice> SearchInvoice(Invoice searchObject)
        {
            throw new NotImplementedException();
        }

        public bool Save(Invoice invoice)
        {
            InvoiceSaved = true;
            invoice.State = BusinessObjectState.Unmodified;
            return true;
        }

        public bool Delete(Invoice invoice)
        {
            throw new NotImplementedException();
        }
    }
}
