using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_Client.Controls;

namespace ERP_Client.ViewModels
{
    public class KontaktAutoCompleteSource : IAutoCompleteSource
    {
        public IEnumerable GetItems(string searchExpression)
        {
            Proxy proxy = new Proxy();
            Contact contact = new Contact();

            contact.State = "SearchObject";
            contact.Firstname = searchExpression;
            contact.Lastname = searchExpression;

            var list = proxy.KontaktSuchen(contact);

            contact.Firstname = null;
            contact.Lastname = null;
            contact.Name = searchExpression;

            list.AddRange(proxy.KontaktSuchen(contact));

            return list;
        }
    }
}
