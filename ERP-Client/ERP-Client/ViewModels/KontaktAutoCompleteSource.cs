﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPClient.Controls;
using ERPClient.Fenster;

namespace ERPClient.ViewModels
{
    public class KontaktAutoCompleteSource : IAutoCompleteSource
    {
        public virtual IEnumerable GetItems(string searchExpression)
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

            list.Concat(proxy.KontaktSuchen(contact));

            return list;
        }


        public string SearchInWindow(string searchString)
        {
            var window = new Kontakte(true, searchString);
            window.ShowDialog();
            return window.Result;
        }
    }
}
