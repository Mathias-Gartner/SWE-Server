using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP_Client.ViewModels.FensterModels
{
    public class AddKontaktViewModel : KontaktViewModel
    {
        public AddKontaktViewModel() { }

        public AddKontaktViewModel(Contact c)
        {
            neu = false;
            Id = c.ID;
            Firstname = c.Firstname;
            Lastname = c.Lastname;
            BornDate = (c.DateOfBirth.HasValue ? c.DateOfBirth.Value : DateTime.Now.Date);
            Prefix = c.Prefix;
            Suffix = c.Suffix;
            Firmname = c.Name;
            Uid = c.Uid;

            if (c.Address != null)
            {
                Streetname = c.Address.Street;
                Number = c.Address.Number;
                PostalCode = c.Address.PostalCode;
                PostOfficeBox = c.Address.PostOfficeBox;
                City = c.Address.City;
                Country = c.Address.Country;
            }

            if (c.InvoiceAddress != null)
            {
                RStreetname = c.InvoiceAddress.Street;
                RNumber = c.InvoiceAddress.Number;
                RPostalCode = c.InvoiceAddress.PostalCode;
                RPostOfficeBox = c.InvoiceAddress.PostOfficeBox;
                RCity = c.InvoiceAddress.City;
                RCountry = c.InvoiceAddress.Country;
            }

            if (c.DeliveryAddress != null)
            {
                LStreetname = c.DeliveryAddress.Street;
                LNumber = c.DeliveryAddress.Number;
                LPostalCode = c.DeliveryAddress.PostalCode;
                LPostOfficeBox = c.DeliveryAddress.PostOfficeBox;
                LCity = c.DeliveryAddress.City;
                LCountry = c.DeliveryAddress.Country;
            }

            if (c.BelongsTo != null)
                BelongsTo = c.BelongsTo;

            Email = c.Email;
        }

        private string _Changeresult = "Status";
        public string Changeresult
        {
            get
            {
                return _Changeresult;
            }
            set
            {
                if (_Changeresult != value)
                {
                    _Changeresult = value;
                    OnPropertyChanged("Changeresult");
                }
            }
        }

        private ICommandViewModel _KontaktChange;
        public ICommandViewModel KontaktChange
        {
            get
            {
                if (_KontaktChange == null)
                {
                    _KontaktChange = new ExecuteCommandViewModel(
                        "Ändern/Hinzufügen",
                        "Kontaktdaten ändern/hinzufügen",
                        Change);
                }
                return _KontaktChange;
            }
        }

        #region Methoden
        public void Change()
        {
            Proxy proxy = new Proxy();
            Contact contact = new Contact();
            string result;

            if (neu)
                contact.State = "New";
            else
            {
                contact.State = "Modified";
                contact.ID = Id;
            }

            if (IsFirma == false)
            {
                contact.Firstname = Firstname;
                contact.Lastname = Lastname;
                contact.Prefix = Prefix;
                contact.Suffix = Suffix;
                contact.DateOfBirth = BornDate;
            }

            if (IsFirma == true)
            {
                contact.Name = Firmname;
                contact.Uid = Uid;
            }

            contact.Address = new Address();
            contact.Address.Street = Streetname;
            contact.Address.Number = Number;
            contact.Address.PostalCode = PostalCode;
            contact.Address.PostOfficeBox = PostOfficeBox;
            contact.Address.City = City;
            contact.Address.Country = Country;

            contact.InvoiceAddress = new Address();
            contact.InvoiceAddress.Street = RStreetname;
            contact.InvoiceAddress.Number = RNumber;
            contact.InvoiceAddress.PostalCode = RPostalCode;
            contact.InvoiceAddress.PostOfficeBox = RPostOfficeBox;
            contact.InvoiceAddress.City = RCity;
            contact.InvoiceAddress.Country = RCountry;

            contact.DeliveryAddress = new Address();
            contact.DeliveryAddress.Street = LStreetname;
            contact.DeliveryAddress.Number = LNumber;
            contact.DeliveryAddress.PostalCode = LPostalCode;
            contact.DeliveryAddress.PostOfficeBox = LPostOfficeBox;
            contact.DeliveryAddress.City = LCity;
            contact.DeliveryAddress.Country = LCountry;

            if (BelongsTo != null && BelongsTo is Contact)
            {
                contact.BelongsTo = (Contact)BelongsTo;
            }
            else
            {
                contact.BelongsTo = null;
            }

            contact.Email = Email;

            result = proxy.KontaktChange(contact);

            if (result.ToLower().Contains("true"))
                Changeresult = "Kontakt gespeichert";
            else
                Changeresult = "Beim Speichern ist ein Fehler aufgetreten";
        }
        #endregion

        private FirmaAutoCompleteSource _firmaAutoCompleteSource;
        public FirmaAutoCompleteSource FirmaAutoCompleteSource
        {
            get
            {
                if (_firmaAutoCompleteSource == null)
                    _firmaAutoCompleteSource = new FirmaAutoCompleteSource();
                return _firmaAutoCompleteSource;
            }
        }

        public object BelongsTo
        {
            get { return GetValue(DPBelongsTo); }
            set { SetValue(DPBelongsTo, value); }
        }

        public static readonly DependencyProperty DPBelongsTo =
            DependencyProperty.Register("BelongsTo", typeof(object), typeof(KontaktViewModel));
    }
}
