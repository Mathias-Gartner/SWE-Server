using ERP_Client.Fenster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP_Client.ViewModels
{
    public class SingleContactViewModel : ViewModel
    {
        public SingleContactViewModel(Contact c)
        {
            Contact = c;
            DisplayName = c.Firstname + " " + c.Lastname + " " + c.Name + " " + c.Uid;
        }

        public Contact Contact { get; set; }

        public string DisplayName
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(SingleContactViewModel));

        private ICommand _editContact;
        public ICommand EditContact
        {
            get
            {
                if (_editContact == null)
                    _editContact = new EditContactCommand(Contact);
                return _editContact;
            }
        }

        private class EditContactCommand : ICommand
        {
            private Contact contact;

            public EditContactCommand(Contact contact)
            {
                this.contact = contact;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                AddKontakt addKontakt = new AddKontakt(contact);
                addKontakt.ShowDialog();
            }
        }
    }
}
