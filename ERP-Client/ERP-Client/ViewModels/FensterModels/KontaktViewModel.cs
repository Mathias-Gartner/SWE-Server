using System;
using System.Collections.Generic;
using System.Linq;
using ERP_Client.Fenster;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;


namespace ERP_Client.ViewModels.FensterModels
{
    public class KontaktViewModel : ViewModel
    {
        bool neu = true;

        public KontaktViewModel()
        {
        }

        public KontaktViewModel(Contact c)
        {
            neu = false;
            Id = c.ID;
            Firstname = c.Firstname;
            Lastname = c.Lastname;
            BornDate = (c.DateOfBirth.HasValue ? c.DateOfBirth.ToString() : string.Empty);
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

        #region Kontaktdaten

        #region Person
        private string _Firstname;
        public string Firstname
        {
            get
            {
                return _Firstname;
            }
            set
            {
                if (_Firstname != value)
                {
                    _Firstname = value;
                    OnPropertyChanged("Firstname");
                    NotifyStateChanged();
                }
            }
        }

        private string _Lastname;
        public string Lastname
        {
            get
            {
                return _Lastname;
            }
            set
            {
                if (_Lastname != value)
                {
                    _Lastname = value;
                    OnPropertyChanged("Lastname");
                    NotifyStateChanged();
                }
            }
        }

        private string _Prefix;
        public string Prefix
        {
            get
            {
                return _Prefix;
            }
            set
            {
                if (_Prefix != value)
                {
                    _Prefix = value;
                    OnPropertyChanged("Prefix");
                    NotifyStateChanged();
                }
            }
        }

        private string _Suffix;
        public string Suffix
        {
            get
            {
                return _Suffix;
            }
            set
            {
                if (_Suffix != value)
                {
                    _Suffix = value;
                    OnPropertyChanged("Suffix");
                    NotifyStateChanged();
                }
            }
        }

        #endregion

        #region Firma
        private string _Firmname;
        public string Firmname
        {
            get
            {
                return _Firmname;
            }
            set
            {
                if (_Firmname != value)
                {
                    _Firmname = value;
                    OnPropertyChanged("Firmname");
                    NotifyStateChanged();
                }
            }
        }

        private string _Uid;
        public string Uid
        {
            get
            {
                return _Uid;
            }
            set
            {
                if (_Uid != value)
                {
                    _Uid = value;
                    OnPropertyChanged("Uid");
                    NotifyStateChanged();
                }
            }
        }
        #endregion

        #region weitere Angaben

        private string _Id;
        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        #region Adresse
        private string _Streetname;
        public string Streetname
        {
            get
            {
                return _Streetname;
            }
            set
            {
                if (_Streetname != value)
                {
                    _Streetname = value;
                    OnPropertyChanged("Streetname");
                }
            }
        }

        private string _Number;
        public string Number
        {
            get
            {
                return _Number;
            }
            set
            {
                if (_Number != value)
                {
                    _Number = value;
                    OnPropertyChanged("Number");
                }
            }
        }

        private string _PostOfficeBox;
        public string PostOfficeBox
        {
            get
            {
                return _PostOfficeBox;
            }
            set
            {
                if (_PostOfficeBox != value)
                {
                    _PostOfficeBox = value;
                    OnPropertyChanged("PostOfficeBox");
                }
            }
        }

        private string _PostalCode;
        public string PostalCode
        {
            get
            {
                return _PostalCode;
            }
            set
            {
                if (_PostalCode != value)
                {
                    _PostalCode = value;
                    OnPropertyChanged("PostalCode");
                }
            }
        }

        private string _City;
        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _Country;
        public string Country
        {
            get
            {
                return _Country;
            }
            set
            {
                if (_Country != value)
                {
                    _Country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        #endregion

        #region Rechnungsadresse
        private string _RStreetname;
        public string RStreetname
        {
            get
            {
                return _RStreetname;
            }
            set
            {
                if (_RStreetname != value)
                {
                    _RStreetname = value;
                    OnPropertyChanged("RStreetname");
                }
            }
        }

        private string _RNumber;
        public string RNumber
        {
            get
            {
                return _RNumber;
            }
            set
            {
                if (_RNumber != value)
                {
                    _RNumber = value;
                    OnPropertyChanged("RNumber");
                }
            }
        }

        private string _RPostOfficeBox;
        public string RPostOfficeBox
        {
            get
            {
                return _RPostOfficeBox;
            }
            set
            {
                if (_RPostOfficeBox != value)
                {
                    _RPostOfficeBox = value;
                    OnPropertyChanged("RPostOfficeBox");
                }
            }
        }

        private string _RPostalCode;
        public string RPostalCode
        {
            get
            {
                return _RPostalCode;
            }
            set
            {
                if (_RPostalCode != value)
                {
                    _RPostalCode = value;
                    OnPropertyChanged("RPostalCode");
                }
            }
        }

        private string _RCity;
        public string RCity
        {
            get
            {
                return _RCity;
            }
            set
            {
                if (_RCity != value)
                {
                    _RCity = value;
                    OnPropertyChanged("RCity");
                }
            }
        }

        private string _RCountry;
        public string RCountry
        {
            get
            {
                return _RCountry;
            }
            set
            {
                if (_RCountry != value)
                {
                    _RCountry = value;
                    OnPropertyChanged("RCountry");
                }
            }
        }
        #endregion

        #region Lieferadresse
        private string _LStreetname;
        public string LStreetname
        {
            get
            {
                return _LStreetname;
            }
            set
            {
                if (_LStreetname != value)
                {
                    _LStreetname = value;
                    OnPropertyChanged("LStreetname");
                }
            }
        }

        private string _LNumber;
        public string LNumber
        {
            get
            {
                return _LNumber;
            }
            set
            {
                if (_LNumber != value)
                {
                    _LNumber = value;
                    OnPropertyChanged("LNumber");
                }
            }
        }

        private string _LPostOfficeBox;
        public string LPostOfficeBox
        {
            get
            {
                return _LPostOfficeBox;
            }
            set
            {
                if (_LPostOfficeBox != value)
                {
                    _LPostOfficeBox = value;
                    OnPropertyChanged("LPostOfficeBox");
                }
            }
        }

        private string _LPostalCode;
        public string LPostalCode
        {
            get
            {
                return _LPostalCode;
            }
            set
            {
                if (_LPostalCode != value)
                {
                    _LPostalCode = value;
                    OnPropertyChanged("LPostalCode");
                }
            }
        }

        private string _LCity;
        public string LCity
        {
            get
            {
                return _LCity;
            }
            set
            {
                if (_LCity != value)
                {
                    _LCity = value;
                    OnPropertyChanged("LCity");
                }
            }
        }

        private string _LCountry;
        public string LCountry
        {
            get
            {
                return _LCountry;
            }
            set
            {
                if (_LCountry != value)
                {
                    _LCountry = value;
                    OnPropertyChanged("LCountry");
                }
            }
        }
        #endregion

        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        private string _FirmSuche;
        public string FirmSuche
        {
            get
            {
                return _FirmSuche;
            }
            set
            {
                if (_FirmSuche != value)
                {
                    _FirmSuche = value;
                    OnPropertyChanged("FirmSuche");
                }
            }
        }
        #endregion

        private string _Suchergebnis = "Suchergebnis";
        public string Suchergebnis
        {
            get
            {
                return _Suchergebnis;
            }
            set
            {
                if (_Suchergebnis != value)
                {
                    _Suchergebnis = value;
                    OnPropertyChanged("Suchergebnis");
                }
            }
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

        private string _BornDate;
        private DateTime Date;
        public string BornDate
        {
            get
            {
                return _BornDate;
            }
            set
            {
                if (_BornDate != value)
                {
                    _BornDate = value;
                    if (!string.IsNullOrEmpty(_BornDate))
                        Date = DateTime.Parse(_BornDate);
                    OnPropertyChanged("BornDate");
                }
            }
        }

        #endregion

        public IEnumerable<SingleContactViewModel> Contacts
        {
            get { return (IEnumerable<SingleContactViewModel>)GetValue(DPContacts); }
            set { SetValue(DPContacts, value); }
        }

        public static readonly DependencyProperty DPContacts =
            DependencyProperty.Register("Contacts", typeof(IEnumerable<SingleContactViewModel>), typeof(KontaktViewModel));

        private IEnumerable<Contact> _kontaktliste;

        public IEnumerable<Contact> Kontaktliste
        {
            get
            {
                return _kontaktliste;
            }
            set
            {
                if (_kontaktliste != value)
                {
                    _kontaktliste = value;
                    Contacts = value.Select(c => new SingleContactViewModel(c));
                    OnPropertyChanged("Kontaktliste");
                }
            }
        }

        #region Buttons

        private ICommandViewModel _KontaktSuche;
        public ICommandViewModel KontaktSuche
        {
            get
            {
                if (_KontaktSuche == null)
                {
                    _KontaktSuche = new ExecuteCommandViewModel(
                        "Suchen",
                        "Kontaktsuche starten",
                        Suche);
                }
                return _KontaktSuche;
            }
        }

        private ICommandViewModel _FirmaSuche;
        public ICommandViewModel FirmaSuche
        {
            get
            {
                if (_FirmaSuche == null)
                {
                    _FirmaSuche = new ExecuteCommandViewModel(
                        "Suchen",
                        "Kontaktsuche starten",
                        SucheFirma);
                }
                return _FirmaSuche;
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

        private ICommandViewModel _OpenAdd;
        public ICommandViewModel OpenAdd
        {
            get
            {
                if (_OpenAdd == null)
                {
                    _OpenAdd = new ExecuteCommandViewModel(
                        "Neuer Kontakt",
                        "Neuer Kontakten erstellen",
                        OpenAddKontakt);
                }
                return _OpenAdd;
            }
        }
        #endregion

        #region Methoden

        #region Suche
        public void Suche()
        {
            Proxy proxy = new Proxy();
            Contact contact = new Contact();
            int anzahl = 0;
            string text;

            contact.State = "SearchObject";
            if (IsFirma == false)
            {
                contact.Firstname = Firstname;
                contact.Lastname = Lastname;
            }
            else
            {
                contact.Name = Firmname;
                contact.Uid = Uid;
            }

            Kontaktliste = proxy.KontaktSuchen(contact);

            text = "Suchergebnis: ";

            if (Kontaktliste != null)
                anzahl = Kontaktliste.Count();

            if (anzahl < 1)
                text += "Keine Kontakte gefunden.";

            Suchergebnis = text;
        }

        public void SucheFirma()
        {
            Proxy proxy = new Proxy();
            Contact contact = new Contact();
            int anzahl = 0;

            contact.State = "SearchObject";

            if (IsFirma == true)
                contact.Name = FirmSuche;

            Kontaktliste = proxy.KontaktSuchen(contact);

            if (Kontaktliste != null)
                anzahl = Kontaktliste.Count();

            //if (anzahl < 1)
        }
        #endregion

        #region Kontakt ändern
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
                if (!string.IsNullOrEmpty(BornDate))
                    contact.DateOfBirth = Date;
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

            contact.Email = Email;

            result = proxy.KontaktChange(contact);

            Changeresult = result;
        }
        #endregion

        #region Neuer Kontakt-Fenster öffnen

        public void OpenAddKontakt()
        {
            var dlg = new ERP_Client.Fenster.AddKontakt();
            dlg.ShowDialog();
        }
        #endregion
        #endregion

        #region View
        public bool? IsFirma
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Lastname) && string.IsNullOrWhiteSpace(Firstname) && string.IsNullOrWhiteSpace(Firmname) && string.IsNullOrWhiteSpace(Uid)
                    && string.IsNullOrWhiteSpace(Prefix) && string.IsNullOrWhiteSpace(Suffix)) return null;
                return !(string.IsNullOrWhiteSpace(Firmname) && string.IsNullOrWhiteSpace(Uid));
            }
        }

        public bool CanEditPerson
        {
            get
            {
                return IsFirma == null || IsFirma == false;
            }
        }

        public bool CanEditFirm
        {
            get
            {
                return IsFirma == null || IsFirma == true;
            }
        }

        private void NotifyStateChanged()
        {
            OnPropertyChanged("IsFirma");
            OnPropertyChanged("CanEditPerson");
            OnPropertyChanged("CanEditFirm");
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
        //, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, BelongsTo_Changed));

        /*private static void BelongsTo_Changed(DependencyObject autoComplete, DependencyPropertyChangedEventArgs e)
        {
            var control = (AutoComplete) autoComplete;
            if (e.NewValue != null)
                control.textBox.Text = e.NewValue.ToString();
        }*/
    }
}
