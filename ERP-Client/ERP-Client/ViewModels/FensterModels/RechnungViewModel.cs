using System;
using System.Collections.Generic;
using System.Linq;
using ERPClient.Fenster;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ERPClient.ViewModels.FensterModels
{
    public class RechnungViewModel : ViewModel
    {
        public RechnungViewModel() { }

        public RechnungViewModel(Invoice Invoice)
        {
            InvoiceDate = Convert.ToDateTime(Invoice.InvoiceDate);
            DueDate = Convert.ToDateTime(Invoice.DueDate);
            Message = Invoice.Message;
            Comment = Invoice.Comment;
            BelongsTo = Invoice.Contact;

            foreach (InvoiceEntry i in Invoice.Entries)
                Rechnungszeile.Add(i);            
        }

        public Action CloseAction { get; set; }

        public bool SearchMode
        {
            get { return (bool)GetValue(SearchModeProperty); }
            protected set { SetValue(SearchModeProperty, value); }
        }

        public static readonly DependencyProperty SearchModeProperty =
            DependencyProperty.Register("SearchMode", typeof(bool), typeof(RechnungViewModel));

        public void SetSearchResult(Contact result)
        {
            if (result == null)
                Result = null;
            else
                Result = result.ToString();

            if (CloseAction != null)
                CloseAction();
        }

        public string Result { get; protected set; }

        protected bool Neu { get; set; }

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

        private string _Ust;
        public string Ust
        {
            get
            {
                return _Ust;
            }
            set
            {
                if (_Ust != value)
                {
                    _Ust = value;
                    OnPropertyChanged("Ust");
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

            var invoice = new Invoice();
            invoice.State = "SearchObject";
            invoice.InvoiceDateFrom = Begindatum;
            invoice.InvoiceDateTo = Enddatum;
            
            if(MinBetrag != null)
                invoice.SumFrom = Convert.ToInt32(MinBetrag);
            if (MaxBetrag != null)
                invoice.SumTo = Convert.ToInt32(MaxBetrag);

            if (BelongsTo != null && BelongsTo is Contact)
            {
                invoice.Contact = (Contact)BelongsTo;
            }
            else
            {
                invoice.Contact = null;
            }

            var liste = proxy.RechnungSuchen(invoice);
            
            Rechnungsliste = liste;
        }

        public void ErstelleRechnung()
        {
            var dlg = new ERPClient.Fenster.NewRechnung();
            dlg.ShowDialog();
        }

        public ERPClient.Fenster.AddRechnungszeile dlgr;

        public void ErstelleRechnungszeile()
        {            
            dlgr = new ERPClient.Fenster.AddRechnungszeile(this);
            dlgr.ShowDialog();
        }

        public void RechnungszeileHinzufügen()
        {
            InvoiceEntry zeile = new InvoiceEntry();
            zeile.Description = Productname;
            zeile.Amount = Convert.ToInt32(Amount);
            zeile.Price = Convert.ToDecimal(Price);
            zeile.UStPercent = Convert.ToInt32(Ust);
            Rechnungszeile.Add(zeile);
            OnPropertyChanged("Rechnungszeile");
            
            dlgr.Close();
        }

        public void RechnungSpeichern()
        {
            Proxy proxy = new Proxy();
            var invoice = new Invoice();

            invoice.InvoiceDate = InvoiceDate;
            invoice.DueDate = DueDate;
            invoice.Comment = Comment;
            invoice.Message = Message;

            if (BelongsTo != null && BelongsTo is Contact)
            {
                invoice.Contact = (Contact)BelongsTo;
            }
            else
            {
                invoice.Contact = null;
            }

            foreach (InvoiceEntry rz in Rechnungszeile)
                invoice.Entries.Add(rz);

            proxy.RechnungChange(invoice);
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

        private List<InvoiceEntry> _Rechnungszeile;
        public List<InvoiceEntry> Rechnungszeile
        {
            get
            {
                if (_Rechnungszeile == null)
                    _Rechnungszeile = new List<InvoiceEntry>();
                return _Rechnungszeile;
            }
            set
            {
                if (_Rechnungszeile != value)
                {
                    _Rechnungszeile = value;
                    OnPropertyChanged("Rechnungszeile");
                }
            }
        }

        public IEnumerable<SingleRechnungViewModel> Entries
        {
            get { return (IEnumerable<SingleRechnungViewModel>)GetValue(DPEntries); }
            set { SetValue(DPEntries, value); }
        }

        public static readonly DependencyProperty DPEntries =
            DependencyProperty.Register("Entries", typeof(IEnumerable<SingleRechnungViewModel>), typeof(RechnungViewModel));
        
        private IEnumerable<Invoice> _rechnungsliste;
        public IEnumerable<Invoice> Rechnungsliste
        {
            get
            {
                return _rechnungsliste;
            }
            set
            {
                if (_rechnungsliste != value)
                {
                    _rechnungsliste = value;
                    Entries = value.Select(c => new SingleRechnungViewModel(this, c));
                    OnPropertyChanged("Rechnungsliste");
                }
            }
        }

        public object BelongsTo
        {
            get { return GetValue(DPBelongsTo); }
            set { SetValue(DPBelongsTo, value); }
        }

        public static readonly DependencyProperty DPBelongsTo =
            DependencyProperty.Register("BelongsTo", typeof(object), typeof(RechnungViewModel));
    }
}
