using ERP_Client.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Client.ViewModels
{
    public class FirmaAutoCompleteSource : KontaktAutoCompleteSource
    {
        public override IEnumerable GetItems(string searchExpression)
        {
            Proxy proxy = new Proxy();
            Contact contact = new Contact();

            contact.State = "SearchObject";
            contact.Name = searchExpression;

            var list = proxy.KontaktSuchen(contact);
            return list;
        }
    }
}
