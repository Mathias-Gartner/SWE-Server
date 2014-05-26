using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Client.ViewModels.FensterModels
{
    class RechnungViewModel : ViewModel
    {
        public RechnungViewModel() { }

        #region Daten
        private DateTime _Begindatum = DateTime.Now.Date;
        public DateTime Begindatum
        {
            get
            {
                return _Begindatum;
            }
            set
            {
                if (_Begindatum != value)
                {
                    _Begindatum = value;
                    OnPropertyChanged("Begindatum");
                }
            }
        }

        private DateTime _Enddatum = DateTime.Now.Date;
        public DateTime Enddatum
        {
            get
            {
                return _Enddatum;
            }
            set
            {
                if (_Enddatum != value)
                {
                    _Enddatum = value;
                    OnPropertyChanged("Enddatum");
                }
            }
        }

        private string _MinBetrag;
        public string MinBetrag
        {
            get
            {
                return _MinBetrag;
            }
            set
            {
                if (_MinBetrag != value)
                {
                    _MinBetrag = value;
                    OnPropertyChanged("MinBetrag");
                }
            }
        }

        private string _MaxBetrag;
        public string MaxBetrag
        {
            get
            {
                return _MaxBetrag;
            }
            set
            {
                if (_MaxBetrag != value)
                {
                    _MaxBetrag = value;
                    OnPropertyChanged("MaxBetrag");
                }
            }
        }

        private string _Kontaktname;
        public string Kontaktname
        {
            get
            {
                return _Kontaktname;
            }
            set
            {
                if (_Kontaktname != value)
                {
                    _Kontaktname = value;
                    OnPropertyChanged("Kontaktname");
                }
            }
        }
        #endregion

        #region Buttons
        private ICommandViewModel _RechnungSuche;
        public ICommandViewModel RechnungSuche
        {
            get
            {
                if (_RechnungSuche == null)
                {
                    _RechnungSuche = new ExecuteCommandViewModel(
                        "Suchen",
                        "Rechnungssuche starten",
                        SucheRechnung);
                }
                return _RechnungSuche;
            }
        }

        private ICommandViewModel _OpenCreate;
        public ICommandViewModel OpenCreate
        {
            get
            {
                if (_OpenCreate == null)
                {
                    _OpenCreate = new ExecuteCommandViewModel(
                        "Neue Rechnung",
                        "Neue Rechnung erstellen",
                        ErstelleRechnung);
                }
                return _OpenCreate;
            }
        }
        #endregion

        #region Methoden
        public void SucheRechnung()
        {
            Proxy proxy = new Proxy();

            proxy.RechnungSuche();
        }

        public void ErstelleRechnung()
        {
            Proxy proxy = new Proxy();

            proxy.RechnungErstellen();
        }
        #endregion

        private KontaktAutoCompleteSource _kontaktAutoCompleteSource;
        public KontaktAutoCompleteSource KontaktAutoCompleteSource
        {
            get
            {
                if (_kontaktAutoCompleteSource == null)
                    _kontaktAutoCompleteSource = new KontaktAutoCompleteSource();
                return _kontaktAutoCompleteSource;
            }
        }
    }
}
