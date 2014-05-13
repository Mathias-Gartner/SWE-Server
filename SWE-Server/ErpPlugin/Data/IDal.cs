using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data
{
    public interface IDal
    {
        ICollection<T> SearchBusinessObject<T>(T searchObject) where T : BusinessObject;
        bool SaveBusinessObject<T>(T businessObject) where T : BusinessObject;
        bool DeleteBusinessObject<T>(T businessObject) where T : BusinessObject;

        ICollection<User> SearchUsers(User searchObject);

        ICollection<Contact> SearchContacts(Contact searchObject);
        bool SaveContact(Contact contact);
        bool DeleteContact(Contact contact);

        ICollection<Invoice> SearchInvoice(Invoice searchObject);
        bool SaveInvoice(Invoice invoice);
        bool DeleteInvoice(Invoice invoice);
    }
}
