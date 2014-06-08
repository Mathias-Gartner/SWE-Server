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

        ICollection<User> Search(User searchObject);

        ICollection<Contact> Search(Contact searchObject);
        bool Save(Contact contact);
        bool Delete(Contact contact);

        ICollection<Invoice> SearchInvoice(Invoice searchObject);
        bool Save(Invoice invoice);
        bool Delete(Invoice invoice);
    }
}
