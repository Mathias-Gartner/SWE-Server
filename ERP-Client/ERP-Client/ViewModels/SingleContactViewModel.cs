using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP_Client.ViewModels
{
    public class SingleContactViewModel : ViewModel
    {
        public SingleContactViewModel(Contact c)
        {
            DisplayName = c.Firstname + " " + c.Lastname + " " + c.Name + " " + c.Uid;
        }

        public string DisplayName
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(SingleContactViewModel));
    }
}
