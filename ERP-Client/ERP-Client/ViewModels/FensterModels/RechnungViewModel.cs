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

        private string _Productname;
        public string Productname
        {
            get
            {
                return _Productname;
            }
            set
            {
                if (_Productname != value)
                {
                    _Productname = value;
                    OnPropertyChanged("Productname");
                }
            }
        }

        private string _Amount;
        public string Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        private string _Price;
        public string Price
        {
            get
            {
                return _Price;
            }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        private string _Message;
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private string _Comment;
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                if (_Comment != value)
                {
                    _Comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        private DateTime _InvoiceDate = DateTime.Now.Date;
        public DateTime InvoiceDate
        {
            get
            {
                return _InvoiceDate;
            }
            set
            {
                if (_InvoiceDate != value)
                {
                    _InvoiceDate = value;
                    OnPropertyChanged("InvoiceDate");
                }
            }
        }

        private DateTime _DueDate = DateTime.Now.Date;
        public DateTime DueDate
        {
            get
            {
                return _DueDate;
            }
            set
            {
                if (_DueDate != value)
                {
                    _DueDate = value;
                    OnPropertyChanged("DueDate");
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

        private ICommandViewModel _NewRechnungszeile;
        public ICommandViewModel NewRechnungszeile
        {
            get
            {
                if (_NewRechnungszeile == null)
                {
                    _NewRechnungszeile = new ExecuteCommandViewModel(
                        "Neue Rechnungszeile",
                        "Neue Rechnungszeile erstellen",
                        ErstelleRechnungszeile);
                }
                return _NewRechnungszeile;
            }
        }

        private ICommandViewModel _AddRechnungszeile;
        public ICommandViewModel AddRechnungszeile
        {
            get
            {
                if (_AddRechnungszeile == null)
                {
                    _AddRechnungszeile = new ExecuteCommandViewModel(
                        "Hinzufügen",
                        "Rechnungszeile hinzufügen",
                        RechnungszeileHinzufügen);
                }
                return _AddRechnungszeile;
            }
        }

        private ICommandViewModel _SaveRechnung;
        public ICommandViewModel SaveRechnung
        {
            get
            {
                if (_SaveRechnung == null)
                {
                    _SaveRechnung = new ExecuteCommandViewModel(
                        "Speichern",
                        "Rechnung speichern",
                        RechnungSpeichern);
                }
                return _SaveRechnung;
            }
        }
        #endregion

        #region Methoden
        public void SucheRechnung()
        {
            Proxy proxy = new Proxy();

           // proxy.RechnungSuche();
        }

        public void ErstelleRechnung()
        {
            var dlg = new ERP_Client.Fenster.NewRechnung();
            dlg.ShowDialog();
        }

        public void ErstelleRechnungszeile()
        {
            var dlg = new ERP_Client.Fenster.AddRechnungszeile();
            dlg.ShowDialog();
        }

        public void RechnungszeileHinzufügen()
        { }

        public void RechnungSpeichern()
        { }
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
