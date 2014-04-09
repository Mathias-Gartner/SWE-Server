using System.Collections.Generic;
using ERP_Client.Fenster;
namespace ERP_Client.ViewModels.FensterModels
{
    class KontaktViewModel : ViewModel
    {
        public IEnumerable<ICommandViewModel> SearchCommands
        {
            get
            {
                return new[] 
                {
                    KontaktSuche,
                };
            }
        }
        /*
        public IEnumerable<ICommandViewModel> ChangeCommands
        {
            get
            {
                return new[] 
                {
                    KontaktChange,
                };
            }
        }
        */
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
                }
            }
        }
        #endregion

        #region weitere Angaben

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

        #endregion

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
        /*
        private ICommandViewModel _KontaktChange;
        public ICommandViewModel KontaktChange
        {
            get
            {
                if(_KontaktChange == null)
                {
                    _KontaktChange = new ExecuteCommandViewModel(
                        "Ändern",
                        "Kontaktsuche ändern",
                        Change);
                }
            }

            return _KontaktAendern;
        }
        */
        #endregion

        #region Methoden

        #region Suche
        public void Suche() {
            Proxy proxy = new Proxy();
            Contact contact = new Contact();
            Contact kontakt = new Contact();
            List<Contact> liste = new List<Contact>();
            int anzahl = 0;
            string text;

            contact.State = "SearchObject";
            contact.Firstname = Firstname;
            contact.Lastname = Lastname;

            liste = proxy.KontaktSuchen(contact);

            text = "Suchergebnis:";

            anzahl = liste.Count;

            if (anzahl > 0)
            {
                for (int i = 0; i < anzahl; i++)
                {
                    kontakt = liste[i];

                    if (kontakt.Name != null || kontakt.Uid != null)
                        text += "\n" + kontakt.Name + " " + kontakt.Uid + " ";
                    if (kontakt.Firstname != null || kontakt.Lastname != null)
                        text += "\n" + kontakt.Firstname + " " + kontakt.Lastname + " ";

                    //text += kontakt.Address.Street + " " + kontakt.Address.Number + " " + kontakt.Address.PostalCode + " " + kontakt.Address.City;
                }
            }
            else
                text += "\nKeine Kontakte gefunden.";

            Suchergebnis = text;
        }
        #endregion

        #region Kontakt ändern
        public void Change()
        {

        }
        #endregion

        #endregion
    }
}
