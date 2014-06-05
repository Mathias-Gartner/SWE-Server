using ERP_Client.Fenster;
using ERP_Client.ViewModels.FensterModels;
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
        public SingleContactViewModel(KontaktViewModel parent, Contact c)
        {
            Parent = parent;
            Contact = c;
            if (c.Firstname != null || c.Lastname != null)
                DisplayName = c.Firstname + " " + c.Lastname;
            if (c.Name != null || c.Uid != null)
                DisplayName = c.Name + " " + c.Uid;
        }

        public KontaktViewModel Parent { get; protected set; }

        public bool SearchMode { get { return Parent.SearchMode; } }

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
                    _editContact = new EditContactCommand(this, Contact);
                return _editContact;
            }
        }

        private ICommand _setSearchresult;
        public ICommand SetSearchResult
        {
            get
            {
                if (_setSearchresult == null)
                    _setSearchresult = new SetSearchResultCommand(this, Contact);
                return _setSearchresult;
            }
        }

        private abstract class SingleContactCommand : ICommand
        {
            protected Contact contact;
            protected SingleContactViewModel singleContactViewModel;

            public SingleContactCommand(SingleContactViewModel singleContactViewModel, Contact contact)
            {
                this.singleContactViewModel = singleContactViewModel;
                this.contact = contact;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public abstract void Execute(object parameter);
        }

        private class EditContactCommand : SingleContactCommand
        {
            public EditContactCommand(SingleContactViewModel singleContactViewModel, Contact contact)
                : base(singleContactViewModel, contact)
            { }

            public override void Execute(object parameter)
            {
                AddKontakt addKontakt = new AddKontakt(contact);
                addKontakt.ShowDialog();
                singleContactViewModel.Parent.SucheKontakt();
            }
        }

        private class SetSearchResultCommand : SingleContactCommand
        {
            public SetSearchResultCommand(SingleContactViewModel singleContactViewModel, Contact contact)
                : base(singleContactViewModel, contact)
            { }

            public override void Execute(object parameter)
            {
                singleContactViewModel.Parent.SetSearchResult(contact);
            }
        }
    }
}
