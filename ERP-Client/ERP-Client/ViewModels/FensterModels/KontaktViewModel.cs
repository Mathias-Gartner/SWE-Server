﻿using System;
using System.Collections.Generic;
using ERP_Client.Fenster;
namespace ERP_Client.ViewModels.FensterModels
{
    class KontaktViewModel : ViewModel
    {
        bool neu = true;

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

        private string _State;
        public string State
        {
            get
            {
                return _State;
            }
            set
            {
                if (_State != value)
                {
                    _State = value;
                    OnPropertyChanged("State");
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
        //private DateTime _Date;
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
                    OnPropertyChanged("BornDate");
                }
            }
        }

        #endregion

        public List<Contact> Kontaktliste;

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

        private ICommandViewModel _KontaktChange;
        public ICommandViewModel KontaktChange
        {
            get
            {
                if(_KontaktChange == null)
                {
                    _KontaktChange = new ExecuteCommandViewModel(
                        "Ändern/Hinzufügen",
                        "Kontaktdaten ändern/hinzufügen",
                        Change);
                }
                return _KontaktChange;
            }            
        }
        
        
        #endregion

        #region Methoden

        #region Suche
        public void Suche() {
            Proxy proxy = new Proxy();
            Contact contact = new Contact();
            Contact kontakt = new Contact();
            int anzahl = 0;
            string text;

            contact.State = "SearchObject";
            contact.Firstname = Firstname;
            contact.Lastname = Lastname;

            Kontaktliste = proxy.KontaktSuchen(contact);

            text = "Suchergebnis: ";

            if (Kontaktliste != null)
                anzahl = Kontaktliste.Count;            

            if (anzahl > 0)
            {
                for (int i = 0; i < anzahl; i++)
                {
                    kontakt = Kontaktliste[i];

                    if (kontakt.Name != null || kontakt.Uid != null)
                        text += "\n" + kontakt.Name + " " + kontakt.Uid + " ";
                    if (kontakt.Firstname != null || kontakt.Lastname != null)
                        text += "\n" + kontakt.Firstname + " " + kontakt.Lastname + " ";

                    //text += kontakt.Address.Street + " " + kontakt.Address.Number + " " + kontakt.Address.PostalCode + " " + kontakt.Address.City;
                }
            }
            else
                text += "Keine Kontakte gefunden.";

            Suchergebnis = text;
        }
        #endregion

        #region Kontakt ändern
        public void Change()
        {
            Proxy proxy = new Proxy();
            Contact contact = new Contact();
            string result;

            //contact.ID = "-1";

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
            
            contact.Email = Email;
            contact.State = State;

            result = proxy.KontaktChange(contact);

            Changeresult = result;
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
    }
}
