using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Fake
{
    public class Dal : IDal
    {
        IEnumerable<Contact> contacts = new[]
            {
                new Contact(){ID = 1, Lastname = "Huber", Firstname = "Hans", DateOfBirth = DateTime.Parse("1960-01-01")},
                new Contact(){ID = 2, Lastname = "Huber", Firstname = "Peter", DateOfBirth = DateTime.Parse("1950-01-01")}
            };

        public ICollection<User> SearchUsers(User searchObject)
        {
            if (searchObject.State == BusinessObject.BusinessObjectState.SearchObject && searchObject.Username == "User1")
            {
                var salt = User.CreateSalt();
                return new[] { new User() { ID = 1, PasswordSalt = salt, PasswordHash = User.CreatePasswordHash("123456", salt), Username = "User1" } };
            }
            return new User[] { };
        }

        public ICollection<Contact> SearchContacts(Contact searchObject)
        {
            if (searchObject.State != BusinessObject.BusinessObjectState.SearchObject)
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

        public bool SaveContact(Contact contact)
        {
            throw new NotImplementedException();
        }

        public bool DeleteContact(Contact contact)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> SearchBusinessObject<T>(T searchObject) where T : BusinessObject
        {
            throw new NotImplementedException();
        }

        public bool SaveBusinessObject<T>(T businessObject) where T : BusinessObject
        {
            throw new NotImplementedException();
        }

        public bool DeleteBusinessObject<T>(T businessObject) where T : BusinessObject
        {
            throw new NotImplementedException();
        }
    }
}
